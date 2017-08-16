using System;
using System.IO;
using System.Net;

namespace CTM_PG.Services
{
    public class BookReader : IBookReader
    {
        private const string  _bookUrl = "http://www.loyalbooks.com/download/text/Railway-Children-by-E-Nesbit.txt";
		public Stream GetBookStream()
		{
			byte[] imageData = null;

            using (var wc = new WebClient())
            {
                try{
                    imageData = wc.DownloadData(_bookUrl);    
                }
				catch (WebException wex)
				{
					if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)
					{
						//TODO
						// error 404, do what you need to do
					}
				}
                catch{
                    //TODO
                }
            }

			return new MemoryStream(imageData);
		}

    }
}
