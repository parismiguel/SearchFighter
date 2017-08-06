using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SearchFighter
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { ".net", "java" };

            if (args.Count() == 0)
            {
                WriteColorLine("No arguments passed to query");
                Console.ReadKey();
                return;
            }

            if (args.Length == 1 && HelpRequired(args[0]))
            {
                WriteColorLine(@"Please input any argument. i.e. SearchFighter.exe ""cats"" ""dogs""");
                Console.ReadKey();
                return;
            }

            Dictionary<string, long> googleResults = new Dictionary<string, long>();
            Dictionary<string, long> bingResults = new Dictionary<string, long>();

            try
            {
                foreach (var item in args)
                {
                    var totalGoogle = SearchGoogle(item);
                    var totalBing = SearchBing(item);

                    googleResults.Add(item, totalGoogle);
                    bingResults.Add(item, totalBing);

                    Console.WriteLine("{0} => Google: {1} || Bing: {2}", item, FormatNumber(totalGoogle), FormatNumber(totalBing));
                }

                Console.WriteLine();

                var maxGoogle = googleResults.FirstOrDefault(x => x.Value == googleResults.Values.Max());
                var maxBing = bingResults.FirstOrDefault(x => x.Value == bingResults.Values.Max());

                string totalResults;

                if (maxGoogle.Value > maxBing.Value)
                {
                    totalResults = maxGoogle.Key;
                }
                else if (maxBing.Value > maxGoogle.Value)
                {
                    totalResults = maxBing.Key;
                }
                else
                {
                    totalResults = "Tie";
                }

                Console.WriteLine("Google Winner: {0}", maxGoogle.Key);
                Console.WriteLine("Bing Winner: {0}", maxBing.Key);

                Console.WriteLine();

                WriteColorLine(string.Format("Total Winner: {0}", totalResults));

                Console.ReadKey();

            }
            catch (Exception ex)
            {
                WriteColorLine(string.Format("Error: {0}", ex.Message));
            }

        }

        /// <summary>
        /// Write a Console line in color
        /// </summary>
        /// <param name="value">Text to write</param>
        static void WriteColorLine(string value)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        private static bool HelpRequired(string param)
        {
            return param == "-h" || param == "--help" || param == "/?";
        }

        /// <summary>
        /// Format a number using thousand separator
        /// </summary>
        /// <param name="number">Value to be formatted</param>
        /// <returns></returns>
        public static string FormatNumber(long number)
        {
            string result = number.ToString("0,0", CultureInfo.InvariantCulture);

            return result;
        }

        public static long SearchGoogle(string query)
        {
            var url = "http://google.com/search?q=" + query;

            string _html = ReadTextFromUrl(url);

            long total = GetTotal(_html, ">About ", " results<");

            return total;

        }

        public static long SearchBing(string query)
        {
            var url = "https://www.bing.com/search?q=" + query;

            string _html = ReadTextFromUrl(url);

            string keyString = @"<span class=""sb_count"">";

            int pFrom = _html.IndexOf(keyString) + keyString.Length;
            int pTo = _html.IndexOf(@" resultados<", _html.IndexOf(keyString));

            String total = _html.Substring(pFrom, pTo - pFrom);
            total = total.Replace(",", "").Replace(".", "");

            return long.Parse(total);
        }

        public static long GetTotal(string _html, string _keyString, string _indeString)
        {
            string keyString = _keyString;

            int pFrom = _html.IndexOf(keyString) + keyString.Length;
            int pTo = _html.LastIndexOf(_indeString);

            String total = _html.Substring(pFrom, pTo - pFrom);
            total = total.Replace(",", "").Replace(".", "");

            return long.Parse(total);
        }

        static string ReadTextFromUrl(string url)
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead(url))
            using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
            {
                return textReader.ReadToEnd();
            }
        }

    }
}
