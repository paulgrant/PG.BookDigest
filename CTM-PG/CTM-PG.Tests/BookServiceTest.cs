using System;
using System.Collections.Generic;
using Xunit;
using CTM_PG.Services;
using Moq;
using System.IO;
using System.Linq;

namespace CTM_PG.Tests
{

    public class BookServiceTests
    {

		[Fact()]
		public void ReturnsNonEmptyWordCollection()
        {
            List<BookWordCount> list2 = new List<BookWordCount>() { new BookWordCount("THE", 1) };
            var testString = @"THE RAILWAY CHILDREN";
            Stream textStream = GenerateStreamFromString(testString);

			var bookReaderMock = new Mock<BookReader>();
			bookReaderMock.Setup(c => c.GetBookStream()).Returns(textStream);

            var wordCountService = new WordCountService();

            var bookService = new BookService(bookReaderMock.Object, wordCountService);
            var wordList = bookService.GetWordCountForPresetBook().ToList();

			Assert.True(wordList.Count > 0, "Word count is invalid");
			Assert.True(wordList.Count == 3, "Word count is invalid");
			Assert.Equal(wordList[0].Word, "THE");
			Assert.Equal(wordList[2].Word, "CHILDREN");
		}

		private static Stream GenerateStreamFromString(string s)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		[Fact()]
		public void CheckMergeListFunctionality()
		{
            var bookService = new BookService();

            List<BookWordCount> masterList = new List<BookWordCount>() {new BookWordCount("THE", 4), new BookWordCount("RAILWAY", 1) };
            List<BookWordCount> list2 = new List<BookWordCount>() { new BookWordCount("THE", 1) };

            bookService.MergeWordCountLists(ref masterList, list2);

			Assert.True(masterList.Count == 2, "Word count is invalid");
			Assert.Equal(masterList[0].Word, "THE");
			Assert.Equal(masterList[0].WordCount, 5);
            Assert.Equal(masterList[1].Word, "RAILWAY");
		}

		[Fact()]
        public void MergeListCanMergeEmptyList()
        {
			var bookService = new BookService();

			List<BookWordCount> masterList = new List<BookWordCount>() { new BookWordCount("THE", 4), new BookWordCount("RAILWAY", 1) };
			List<BookWordCount> list2 = new List<BookWordCount>() { };

			bookService.MergeWordCountLists(ref masterList, list2);

			Assert.True(masterList.Count == 2, "Word count is invalid");
			Assert.Equal(masterList[1].Word, "RAILWAY");
			Assert.Equal(masterList[0].WordCount, 4);
        }


		[Fact()]
		public void MergeListCanMergeNullList()
		{
			var bookService = new BookService();

			List<BookWordCount> masterList = new List<BookWordCount>() { new BookWordCount("THE", 4), new BookWordCount("RAILWAY", 1) };
			List<BookWordCount> list2 = null;

			bookService.MergeWordCountLists(ref masterList, list2);

			Assert.True(masterList.Count == 2, "Word count is invalid");
			Assert.Equal(masterList[1].Word, "RAILWAY");
			Assert.Equal(masterList[0].WordCount, 4);
		}

		
    }
}
