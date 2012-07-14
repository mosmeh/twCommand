using System;
using System.IO;
using System.Xml.Serialization;
using OAuthLib;

namespace twitCmd
{
    class Program
    {
        const string tokenfile = "token";

        static void Main(string[] args)
        {
            var consumer = new Consumer("Xq4JnC92AR0yoLS11VclWA", "urZb3nDEhyFDqDaK7Cjc9mRA3IVe3PtWB5STlzVI8");
            AccessToken accesstoken;
            if (!System.IO.File.Exists("token"))
            {
                var reqtoken = consumer.ObtainUnauthorizedRequestToken("https://api.twitter.com/oauth/request_token", "http://twitter.com/");
                System.Diagnostics.Process.Start(Consumer.BuildUserAuthorizationURL("https://api.twitter.com/oauth/authorize", reqtoken));
                Console.Write("PINコードを入力>");
                accesstoken = consumer.RequestAccessToken(Console.ReadLine(), reqtoken, "https://api.twitter.com/oauth/access_token", "http://twitter.com/");

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

            System.Net.HttpWebResponse resp = consumer.AccessProtectedResource(accesstoken, "https://api.twitter.com/1/statuses/update.json", "POST", "http://twitter.com/",
                    new Parameter[] { new Parameter("status", args[0]) });
        }
    }

    public class Tokens
    {
        public string TokenValue { get; set; }
        public string TokenSecret { get; set; }
    }
}
