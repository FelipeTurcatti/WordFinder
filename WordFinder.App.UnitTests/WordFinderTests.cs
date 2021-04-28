using System;
using System.Collections.Generic;
using Xunit;

namespace WordFinder.App.UnitTests
{
    public class WordFinderTests
    {
        [Fact]
        public void Constructor_given_a_matrix_of_strings_should_throw_exception_if_matrix_size_is_not_64_x_64()
        {
            //Arrange.-
            List<string> matrix = new List<string>()
            {
                "This",
                "Matrix",
                "Is",
                "Not",
                "Right"
            };

            //Act, Assert.-
            Assert.Throws<ArgumentException>(() => { WordFinder target = new WordFinder(matrix); });
        }

        [Fact]
        public void Constructor_given_a_matrix_of_strings_should_accept_a_matrix_size_of_64_x_64()
        {
            //Arrange.-
            List<string> matrix = new List<string>();
            string knownStringWith64Characters = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

            //Generate a 64x64 matrix with known values
            for (int i = 0; i < 64; i++)
                matrix.Add(knownStringWith64Characters);

            //Act.-
            Exception ex = Record.Exception(() => { WordFinder target = new WordFinder(matrix); });

            //Assert.-
            Assert.Null(ex);
        }

        [Fact]
        public void Find_must_return_the_top_10_most_repeated_words_from_the_word_stream()
        {
            //Arrange.-
            //Generate a 64x64 matrix with known values.-
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

            WordFinder target = new WordFinder(matrix);

            //Act.-
            IEnumerable<string> matches = target.Find(wordStream);

            //Assert.-
            Assert.Collection(matches,
                e => e.Equals("chill"),
                e => e.Equals("cold"),
                e => e.Equals("wind"),
                e => e.Equals("wine"),
                e => e.Equals("apple"),
                e => e.Equals("tree"),
                e => e.Equals("knife"),
                e => e.Equals("test"),
                e => e.Equals("ok"),
                e => e.Equals("cool")
                );

            Assert.DoesNotContain("notInResults", matches);
        }

        [Fact]
        public void Find_should_return_an_empty_set_of_strings_If_no_words_were_found()
        {
            //Arrange.-
            List<string> matrix = new List<string>();
            string knownStringWith64Characters = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            List<string> wordStream = new List<string>()
            {
                "BBBB",
                "CCCC",
                "DDDDDD"
            };

            //Generate a 64x64 matrix with known values
            for (int i = 0; i < 64; i++)
                matrix.Add(knownStringWith64Characters);

            WordFinder target = new WordFinder(matrix);

            //Act.-
            IEnumerable<string> matches = target.Find(wordStream);

            //Assert.-
            Assert.Empty(matches);
        }

        [Fact]
        public void Find_given_duplicate_words_it_should_count_them_only_once()
        {
            //Arrange.-
            //Generate a 64x64 matrix with known values.-
            string knownStringWith64Characters = "||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||";
            List<string> matrix = new List<string>()
            {
                "O|||||||||||||||||OK||||||||||||||||||||||||CC||||||||||||||||||",
                "K|||||||C||||||||OK||||||||||||||||||||||||COOL|||||||||||||||||",
                "||||||||O|||||||||||||||||||||||||||||||||||OO|||||||||||||||||O",
                "||||||COOL|||||||||||||||||||||||||||||||COOLL||||||||||||||||||",
                "COOL||||L|||||||||||||||||||||||||||||||||KK|||||||||||||||||COO",
            };
            //OK -> 6 times
            //COOL -> 7 times

            for (int i = 0; i < 59; i++)
                matrix.Add(knownStringWith64Characters);

            List<string> wordStream = new List<string>()
            {
                "OK",
                "COOL"
            };

            WordFinder target = new WordFinder(matrix);

            //Act.-
            IEnumerable<string> matches = target.Find(wordStream);

            //Assert.-
            Assert.Collection(matches,
                e => e.Equals("COOL"),
                e => e.Equals("OK"));
        }
    }
}
