using System;
using OAuthLib;
using twCommand.Utils;

namespace twCommand.R4S
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TwitterService.Authorize();
                if (args.Length != 0)
                {
                    System.Net.HttpWebResponse resp =
                        TwitterService.ConsumerKey.AccessProtectedResource(TwitterService.AccessToken, "http://api.twitter.com/1/report_spam.json", "POST", "http://twitter.com/",
                        new Parameter[] { new Parameter("screen_name", args[0]) });
                    if (((int)resp.StatusCode).ToString()[0] != '2')
                    {
                        Console.WriteLine("失敗しました");
                    }
                }
                else
                {
                    Console.WriteLine("ユーザ名を指定してください");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
