using System;
using Xunit;
using CTM_PG.Services;
using System.Collections.Generic;

namespace CTM_PG.Tests
{
	public class WordCountServiceTests
    {
        public WordCountServiceTests()
        {
        }

		[Theory()]
		[InlineData("THE RAILWAY CHILDREN")]
        [InlineData("THE  RAILWAY  CHILDREN")]
    	public void ReturnsNonEmptyWordCollection(string inputString)
    	{

            var wordService = new WordCountService();
            var wordList = wordService.GetWordCount(inputString);

            Assert.True(wordList.Count>0, "Word count is invalid");
            Assert.True(wordList.Count==3, "Word count is invalid");
            Assert.Equal(wordList[0].Word, "THE");
            Assert.Equal(wordList[2].Word, "CHILDREN");
    	}

    	[Theory()]
    	[InlineData(null)]
    	[InlineData("")]
    	[InlineData("     ")]
    	public void ReturnsEmptyWordCollectionWithWhiteSpaceInput(string inputString)
    	{

			var wordService = new WordCountService();
			var wordList = wordService.GetWordCount(inputString);

			Assert.True(wordList.Count == 0, "Word count is invalid");



		}
    }
}
