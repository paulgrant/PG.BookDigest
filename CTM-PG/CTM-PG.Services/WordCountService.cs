using System;
using System.Linq;
using System.Collections.Generic;

namespace CTM_PG.Services
{
    //TODO - Move to own file!
    public interface IWordCountService{
        List<BookWordCount> GetWordCount(String line);
    }

    public class WordCountService : IWordCountService
    {

        public virtual List<BookWordCount> GetWordCount(String line){
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

                if(wordCountList.Any(x=>x.Word.Equals(cleansedWord, StringComparison.InvariantCultureIgnoreCase)))
                {
    				var obj = wordCountList.FirstOrDefault(x => x.Word.Equals(cleansedWord, StringComparison.InvariantCultureIgnoreCase));
    				obj.WordCount += word.Count();
                }
                else
                {
                    wordCountList.Add(new BookWordCount(cleansedWord, 1));
                }
            }

            return wordCountList;
        }
    }
}
