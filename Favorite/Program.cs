using System;
using OAuthLib;
using twCommand.Utils;

namespace twCommand.Favorite
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
                        TwitterService.ConsumerKey.AccessProtectedResource(TwitterService.AccessToken, 
                        "https://api.twitter.com/1/favorites/create/" + args[0] + ".json", "POST", "http://twitter.com/",null);
                    if (((int)resp.StatusCode).ToString()[0] != '2')
                    {
                        Console.WriteLine("失敗しました");
                    }
                }
                else
                {
                    Console.WriteLine("IDを指定してください");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
