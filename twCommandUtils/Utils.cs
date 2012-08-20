using System;
using System.IO;
using System.Xml.Serialization;
using OAuthLib;

namespace twCommand
{
    public class Tokens
    {
        public string TokenValue { get; set; }
        public string TokenSecret { get; set; }
    }
}

namespace twCommand.Utils
{
    public static class TwitterService
    {
        private static string tokenfile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\token";
        public static Consumer ConsumerKey = new Consumer("Xq4JnC92AR0yoLS11VclWA", "urZb3nDEhyFDqDaK7Cjc9mRA3IVe3PtWB5STlzVI8");
        private static AccessToken accesstoken = null;

        public static bool IsAuthorized()
        {
            return accesstoken != null;
        }

        public static AccessToken Authorize()
        {
            if (!System.IO.File.Exists(tokenfile))
            {
                var reqtoken = ConsumerKey.ObtainUnauthorizedRequestToken("https://api.twitter.com/oauth/request_token", "http://twitter.com/");
                System.Diagnostics.Process.Start(Consumer.BuildUserAuthorizationURL("https://api.twitter.com/oauth/authorize", reqtoken));
                Console.Write("PINコードを入力>");
                accesstoken = ConsumerKey.RequestAccessToken(Console.ReadLine(), reqtoken, "https://api.twitter.com/oauth/access_token", "http://twitter.com/");

                using (var sw = new StreamWriter(tokenfile))
                    new XmlSerializer(typeof(Tokens)).Serialize(sw, new Tokens() { TokenValue = accesstoken.TokenValue, TokenSecret = accesstoken.TokenSecret });
            }
            else
            {
                Tokens tokens;
                using (var sr = new StreamReader(tokenfile))
                    tokens = new XmlSerializer(typeof(Tokens)).Deserialize(sr) as Tokens;
                accesstoken = new AccessToken(tokens.TokenValue, tokens.TokenSecret);
            }

            return accesstoken;
        }

        public static AccessToken AccessToken
        {
            get
            {
                return accesstoken;
            }
        }
    }
}
