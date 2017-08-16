using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CTM_PG.Services;

namespace CTM_PG.Services
{
    public class BookService : IBookService
    {
        private readonly IBookReader _bookReader;
        private readonly IWordCountService _wordCountService;

    	public BookService()
    	{
    		_bookReader = new BookReader();
    		_wordCountService = new WordCountService();
    	}

        public BookService(IBookReader bookReader, IWordCountService wordCountService)
        {
            _bookReader = bookReader ?? new BookReader();
            _wordCountService = wordCountService ?? new WordCountService();
        }

        public IEnumerable<BookWordCount> GetWordCountForPresetBook(){
            
            var bookStream = _bookReader.GetBookStream();
            var wordList = GetWordCountForBook(bookStream);
            return wordList;
        }

        protected IEnumerable<BookWordCount> GetWordCountForBook(Stream bookStream)
        {
            var wordList = new List<BookWordCount>();

    		using (StreamReader sr = new StreamReader(bookStream, System.Text.Encoding.Default))
    		{
    			while (sr.Peek() >= 0)
    			{
                    var line = (sr.ReadLine());
                    var lineWordCountList = _wordCountService.GetWordCount(line);
                    MergeWordCountLists(ref wordList, lineWordCountList);
    			}
    		}

            return wordList;
        }

        //Using threading.
    	protected IEnumerable<BookWordCount> ALT_GetWordCountForBook(Stream bookStream)
    	{
            var wordList = new List<BookWordCount>();
            var bookLines = new string[Int32.MaxValue];  //assign memory here
    		using (StreamReader sr = new StreamReader(bookStream, System.Text.Encoding.Default))
    		{
            var lineCount = 0;
    			while (sr.Peek() >= 0)
    			{
    				bookLines[lineCount] = (sr.ReadLine());
    			}
    		}
    	    
            var pWordList = new ConcurrentBag<BookWordCount>();
            //once all lines are read in //result
			Parallel.For(0, bookLines.Length, x =>
			{
				var lineWordCountList = _wordCountService.GetWordCount(bookLines[x]);
                pWordList.Concat(lineWordCountList);
			});

            //TODO Sort and count all occurances
            //wordList = new CountWordOccurancesFromList(pWordList);
            return wordList;
		}

        public void MergeWordCountLists(ref List<BookWordCount> masterList, List<BookWordCount> newList)
        {
            if (newList == null || newList.Count()==0) return;

            if (masterList == null) throw new ArgumentNullException();

            foreach(var item in newList)
            {
                if(masterList.Any(x=>x.Word.Equals(item.Word, StringComparison.InvariantCultureIgnoreCase)))
                {
    				var obj = masterList.FirstOrDefault(x => x.Word.Equals(item.Word, StringComparison.InvariantCultureIgnoreCase));
                    var index = masterList.IndexOf(obj);
    				obj.WordCount += item.WordCount;
                    obj.IsPrimeNumber = IsPrimeNumber(item.WordCount);
                    masterList[index] = obj;
                }
                else
                {
                    var obj = item;
                    obj.IsPrimeNumber = IsPrimeNumber(item.WordCount);
                    masterList.Add(obj);   
                }
            }
        }

		private static bool IsPrimeNumber(int wordCount)
		{
			if(wordCount > 1)
		    {
				return Enumerable.Range(1, wordCount).Where(x => wordCount % x == 0)
								 .SequenceEqual(new[] { 1, wordCount });
			}

			return false;
		}
    }
}
