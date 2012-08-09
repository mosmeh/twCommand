using System;
using System.IO;
using System.Xml.Serialization;
using OAuthLib;

namespace twCommand
{
    class Program
    {
        static string tokenfile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\token";

        static void Main(string[] args)
        {
            try
            {
                var consumer = new Consumer("Xq4JnC92AR0yoLS11VclWA", "urZb3nDEhyFDqDaK7Cjc9mRA3IVe3PtWB5STlzVI8");
                AccessToken accesstoken = null;
                if (!System.IO.File.Exists(tokenfile))
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
                    if (args.Length != 0)
                    {
                        Tokens tokens;
                        using (var sr = new StreamReader(tokenfile))
                            tokens = new XmlSerializer(typeof(Tokens)).Deserialize(sr) as Tokens;
                        accesstoken = new AccessToken(tokens.TokenValue, tokens.TokenSecret);
                    }
                    else
                    {
                        Console.WriteLine("ツイート内容を指定してください");
                        return;
                    }
                }

                System.Net.HttpWebResponse resp = consumer.AccessProtectedResource(accesstoken, "https://api.twitter.com/1/statuses/update.json", "POST", "http://twitter.com/",
                        new Parameter[] { new Parameter("status", args[0]) });
                if (((int)resp.StatusCode).ToString()[0] != '2')
                {
                    Console.WriteLine("失敗しました");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class Tokens
    {
        public string TokenValue { get; set; }
        public string TokenSecret { get; set; }
    }
}
