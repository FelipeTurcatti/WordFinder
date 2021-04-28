
using System;
using System.Collections.Generic;

namespace WordFinder.App
{ 
    class Program
    {
        static void Main()
        {
            string knownStringWith64Characters = "||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||";
            List<string> matrix = new List<string>()
                {
                    "||||||chill|||||t|||||notInResults|okok|||||c||k||||||||w|||||||",
                    "|||||chill||||||r||||||cold|t||||w||||||||||o||n|||||||wine||||c",
                    "||||||i|||||||||e|||||||||||e|||wind||||||||l||i||||||||n||||||o",
                    "|||apple||||||||e|tree||||||s||||n||||||||||d||f||||apple||||||o",
                    "||||||l||||||||||||||||||test||||d|||||||||knife||||||||||||cool",
                };
            List<string> wordStream = new List<string>()
            {
                "chill", // 3 times
                "cold",// 2 times
                "wind",// 2 times
                "wine",// 2 times
                "apple",// 2 times
                "tree",// 2 times
                "knife",// 2 times
                "test",// 2 times
                "ok",// 2 times
                "cool",// 2 times
                "notInResults"// 1 time
            };

            for (int i = 0; i < 59; i++)
                matrix.Add(knownStringWith64Characters);

            IEnumerable<string> matches = new WordFinder(matrix).Find(wordStream);

            Console.WriteLine("Word stream:");
            wordStream.ForEach(e => Console.WriteLine(e));

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Top 10 most repeated words were:");
            foreach (var match in matches)
            {
                Console.WriteLine(match);
            }            
        }
    }
}