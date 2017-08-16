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

		[Theory()]
		public void ReturnsNonEmptyWordCollection()
		{



            List<BookWordCount> list2 = new List<BookWordCount>() { new BookWordCount("THE", 1) };
            Stream textStream = GenerateStreamFromString(@"THE\tRAILWAY\tCHILDREN");

            var wordServiceMock = new Mock<WordCountService>(MockBehavior.Strict);
            wordServiceMock.Setup(x=>x.GetWordCount(It.IsAny<string>())).Returns(list2);

            var bookReaderMock = new Mock<BookReader>();
            bookReaderMock.Setup(c => c.GetBookStream()).Returns(textStream);

            var bookService = new BookService(bookReaderMock.Object, wordServiceMock.Object);
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

		[Theory()]
		public void CheckMergeListFunctionality()
		{
            var bookService = new BookService();

            List<BookWordCount> masterList = new List<BookWordCount>() {new BookWordCount("THE", 4), new BookWordCount("RAILWAY", 1) };
            List<BookWordCount> list2 = new List<BookWordCount>() { new BookWordCount("THE", 1) };

            bookService.MergeWordCountLists(ref masterList, list2);

			Assert.True(masterList.Count == 2, "Word count is invalid");
			Assert.Equal(masterList[1].Word, "THE");
			Assert.Equal(masterList[1].WordCount, 5);
		}

		[Theory()]
        public void MergeListCanMergeEmptyList()
        {
			var bookService = new BookService();

			List<BookWordCount> masterList = new List<BookWordCount>() { new BookWordCount("THE", 4), new BookWordCount("RAILWAY", 1) };
			List<BookWordCount> list2 = new List<BookWordCount>() { };

			bookService.MergeWordCountLists(ref masterList, list2);

			Assert.True(masterList.Count == 2, "Word count is invalid");
			Assert.Equal(masterList[1].Word, "THE");
			Assert.Equal(masterList[1].WordCount, 4);
        }


		[Theory()]
		public void MergeListCanMergeNullList()
		{
			var bookService = new BookService();

			List<BookWordCount> masterList = new List<BookWordCount>() { new BookWordCount("THE", 4), new BookWordCount("RAILWAY", 1) };
			List<BookWordCount> list2 = null;

			bookService.MergeWordCountLists(ref masterList, list2);

			Assert.True(masterList.Count == 2, "Word count is invalid");
			Assert.Equal(masterList[1].Word, "THE");
			Assert.Equal(masterList[1].WordCount, 4);
		}

		
    }
}
