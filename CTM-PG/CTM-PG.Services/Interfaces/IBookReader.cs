using System;
using System.IO;

namespace CTM_PG.Services
{
	public interface IBookReader
    {
        Stream GetBookStream();
    }
}
