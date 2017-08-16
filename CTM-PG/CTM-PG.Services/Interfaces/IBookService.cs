using System;
using System.Collections.Generic;

namespace CTM_PG.Services
{
	public interface IBookService
    {
        IEnumerable<BookWordCount> GetWordCountForPresetBook();

        void MergeWordCountLists(ref List<BookWordCount> masterList, List<BookWordCount> newList);
    }
}
