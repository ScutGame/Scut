using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZyGames.Test.LoadApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:{0}", ex.Message);
            }
            finally
            {
                Console.Read();
            }
        }

        private static void Run(int runSecond = 60, int userNum = 10)
        {
            Console.WriteLine(@"Press any key to stop...");
            bool isEnd = false;

            var loadTest = new LoadTest<object>(runSecond, userNum, null, null, state =>
            {
                Thread.Sleep(1000);
            }, null);
            Task.Factory.StartNew(() =>
            {
                while (!isEnd)
                {
                    Console.Write("Times={0}, QPS={1}/s, RT={2}, MRT={3}, CMP={4}, Total={5}, TO={6}\r", 
                        loadTest.Result.Times,
                         loadTest.Result.CompletedPerSecond,
                         loadTest.Result.ResponseTime,
                         loadTest.Result.MaxResponseTime,
                         loadTest.Result.Completed,
                         loadTest.Result.Total,
                         loadTest.Result.Timeout
                         );
                    Thread.Sleep(500);
                }
            });
            loadTest.RunTask();
            isEnd = true;
        }
    }
}
