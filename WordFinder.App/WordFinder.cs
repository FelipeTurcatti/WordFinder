using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WordFinder.App
{
    public class WordFinder
    {
        #region Members

        private readonly char[,] _matrix;

        private readonly List<Func<int, int, IEnumerable<string>, IEnumerable<string>>> _searchStrategies;

        #endregion

        #region Constructor

        public WordFinder(IEnumerable<string> matrix)
        {
            EnsureMatrixSize(matrix);
            
            _matrix = new char[matrix.First().Length, matrix.Count()];

            int y = 0;
            while (y < matrix.Count())
            {
                int x = 0;
                foreach (char character in matrix.ElementAt(y).ToCharArray())
                {
                    _matrix[y, x] = character;
                    x++;
                }
                y++;
            }

            //Here we can configure different search strategies, ie diagonal search. This strategies will apply upon every element in matrix to find words
            _searchStrategies = new List<Func<int, int, IEnumerable<string>, IEnumerable<string>>>()
            {
                new Func<int, int, IEnumerable<string>, IEnumerable<string>>(TraverseHorizontally),
                new Func<int, int, IEnumerable<string>, IEnumerable<string>>(TraverseVertically)
            };
        }

        #endregion

        #region Methods

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            SearchResults matches = new SearchResults();

            IEnumerable<string> dedupedWordStream = wordstream.Distinct(); // filter out all dupes from the wordstream first

            for (int x = 0; x < _matrix.GetLength(0); x++)
            {
                for (int y = 0; y < _matrix.GetLength(1); y++)
                {
                    //Search for words from the word stream as it was configured.-
                    _searchStrategies.ForEach(e => matches.AddResult(e.Invoke(x, y, dedupedWordStream)));                    
                }
            }

            return matches.GetTop10MostRepeatedWordsOrEmpty();
        }

        /// <summary>
        /// Traverse the matrix horizontally, from left to right.- 
        /// </summary>
        /// <param name="x">First dimension point where the traverse will start.-</param>
        /// <param name="y">Second dimension point where the traverse will start.-</param>
        /// <param name="wordStream">Set of words to find starting from <paramref name="x"/>,<paramref name="y"/>></param>
        /// <returns>Object representing the search result in matrix.-</returns>
        private IEnumerable<string> TraverseHorizontally(int x, int y, IEnumerable<string> wordStream)
        {
            foreach (string word in wordStream)
            {
                //Check if word lenght fits in array.-
                if (y + word.Length > _matrix.GetLength(0))
                    continue;

                StringBuilder builder = new StringBuilder();
                for (short i = 0; i < word.Length; i++)
                {
                    builder.Append(_matrix[x, y + i]);
                }
                
                if (builder.ToString().Equals(word))
                    yield return word;
            }
        }

        /// <summary>
        /// Traverse the matrix vertically, from top to bottom.-
        /// </summary>
        /// <param name="x">First dimension point where the traverse will start.-</param>
        /// <param name="y">Second dimension point where the traverse will start.-</param>
        /// <param name="wordStream">Set of words to find starting from <paramref name="x"/>,<paramref name="y"/>></param>
        /// <returns>Object representing the search result in matrix.-</returns>
        private IEnumerable<string> TraverseVertically(int x, int y, IEnumerable<string> wordStream)
        {
            foreach (string word in wordStream)
            {
                //Check if word lenght fits in array.-
                if (x + word.Length > _matrix.GetLength(1))
                    continue;

                StringBuilder builder = new StringBuilder();
                for (short i = 0; i < word.Length; i++)
                {
                    builder.Append(_matrix[x + i, y]);
                }
                
                if (builder.ToString().Equals(word))
                    yield return word;
            }
        }

        private void EnsureMatrixSize(IEnumerable<string> matrix)
        {
            if (matrix == null || matrix.Count() != 64)
                throw new ArgumentException("Matrix size must 64x64", nameof(matrix));

            if (matrix.Any(e => string.IsNullOrEmpty(e) || e.Length != 64))
                throw new ArgumentException("Matrix size must 64x64", nameof(matrix));
        }

        #endregion

        private class SearchResults
        {
            #region Members

            private readonly List<string> _results;

            #endregion

            #region Constructor

            public SearchResults()
            {
                _results = new List<string>();
            }

            #endregion

            #region Methods

            public void AddResult(IEnumerable<string> result)
            {
                _results.AddRange(result);
            }

            internal IEnumerable<string> GetTop10MostRepeatedWordsOrEmpty()
            {
                return _results.GroupBy(x => x)
                        .Where(group => group.Count() >= 1)
                        .OrderByDescending(x => x.Count())
                        .Take(10)
                        .Select(group => group.Key);
            }

            #endregion
        }
    }
}
