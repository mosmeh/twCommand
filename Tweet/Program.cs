using System;
using OAuthLib;
using twCommand.Utils;

namespace twCommand.Tweet
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                bool AuthorizedAtFirst = TwitterService.IsAuthorized();
                if (!TwitterService.IsAuthorized())
                    TwitterService.Authorize();
                if (args.Length != 0)
                {
                    System.Net.HttpWebResponse resp =
                        TwitterService.ConsumerKey.AccessProtectedResource(TwitterService.Authorize(), "https://api.twitter.com/1/statuses/update.json", "POST", "http://twitter.com/",
                        new Parameter[] { new Parameter("status", args[0]) });
                    if (((int)resp.StatusCode).ToString()[0] != '2')
                    {
                        Console.WriteLine("失敗しました");
                    }
                }
                else if (AuthorizedAtFirst)
                {
                    Console.WriteLine("ツイート内容を指定してください");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
