using System;
using System.Linq;
using System.Collections.Generic;

namespace CTM_PG.Services
{
    //TODO - Move to own file!
    public interface IWordCountService{
        List<BookWordCount> GetWordCount(string line);
    }

    public class WordCountService : IWordCountService
    {

        public List<BookWordCount> GetWordCount(string line){
            var wordCountList = new List<BookWordCount>();

            if (string.IsNullOrEmpty(line)) return wordCountList;

            //split with null = whitespace
            var words = line.Split(null);

            foreach(var word in words)
            {

                //TODO refactor this into it's own method.  Although short and concise.
                if (string.IsNullOrEmpty(word)) continue;

                //cleanse word, remove all characters that are not word characters
                var cleansedWord = System.Text.RegularExpressions.Regex.Replace(word, @"[^\w]", "");

                if(wordCountList.Any(x=>x.Word.Equals(word, StringComparison.InvariantCultureIgnoreCase)))
                {
    				var obj = wordCountList.FirstOrDefault(x => x.Word.Equals(word, StringComparison.InvariantCultureIgnoreCase));
    				obj.WordCount += word.Count();
                }
                else
                {
                    wordCountList.Add(new BookWordCount(word, 1));
                }
            }

            return wordCountList;
        }
    }
}
