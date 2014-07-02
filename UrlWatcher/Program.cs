using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UrlWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            long run = 0;
            while (true)
            {
                TestEndpoint("http://az590556.vo.msecnd.net/app/ec951196-2be1-4539-9e8f-3fc9f59590a9/client.js", "cdn_result.csv", "cdn_header.txt", ConsoleColor.Yellow, run);
                //TestEndpoint("http://dellpoccdn.blob.core.windows.net/app/ec951196-2be1-4539-9e8f-3fc9f59590a9/client.js", "blob_result.csv", "blob_header.txt", ConsoleColor.Green, run);
                //System.Threading.Thread.Sleep(10);
                run++;
            }
        }

        public static void TestEndpoint(string url, string resultFile, string headerFile, ConsoleColor color, long run)
        {
            string message = string.Empty;
            IEnumerable<string> headers = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var result = client.GetAsync(url).Result;
                    sw.Stop();

                    IEnumerable<string> cache;
                    string cacheString = "NONE";

                    if (result.Headers.TryGetValues("X-Cache", out cache))
                    {
                        cacheString = string.Join("-", cache);
                    }

                    message = string.Format("{0:G},{1},{2},{3},{4}", System.DateTime.Now, result.StatusCode, sw.ElapsedMilliseconds, cacheString, run);
                    headers = result.Headers.Select(x => string.Format("{0}: {1}", x.Key, string.Join("|", x.Value)));
                }
            }
            catch (Exception ex)
            {
                message = string.Format("{0:G},{1},{2},{3},{4}", System.DateTime.Now, "ERROR", 0, "ERROR", run);
                headers = new List<string>() { ex.Message.ToString(), ex.StackTrace.ToString() };
            }

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            File.AppendAllText(resultFile, message + Environment.NewLine);

            File.AppendAllText(headerFile, "----------------------------------------" + Environment.NewLine + string.Format("Run# {0:000000000}", run) + Environment.NewLine);
            File.AppendAllLines(headerFile, headers);
        }
    }
}
