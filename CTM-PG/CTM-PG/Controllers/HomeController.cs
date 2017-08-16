using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CTM_PG.Models;
using CTM_PG.Services;

namespace CTM_PG.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// TODO : Use Unity to inject here!
        /// </summary>
        private readonly IBookService bookService = new BookService();
        public IActionResult Index()
        {
            return View();
        }

		public IActionResult ReadBook1()
		{
            try{
                var list = bookService.GetWordCountForPresetBook();
                return Ok(list);
            } 
            catch{
                //TODO!
            }
            return Error();
		}

		public IActionResult ReadBook2()
		{
			try
			{
				var list = bookService.GetWordCountForPresetBook();
				return Ok(list);
			}
			catch
			{
				//TODO!
			}
			return Error();
		}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
