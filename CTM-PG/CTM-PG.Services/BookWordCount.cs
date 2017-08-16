using System;
using System.Linq;

namespace CTM_PG.Services
{
	public struct BookWordCount
    {
        public BookWordCount(string word, int wordCount)
        {
            Word = word;
            WordCount = wordCount;
            IsPrimeNumber = false;
        }

        public string Word { get; set; }
        public int WordCount { get; set; }
        public bool IsPrimeNumber { get; set; }
    }
}
