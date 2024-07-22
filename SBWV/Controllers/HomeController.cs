using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBWV.Models;
using SBWV.Models.ViewModels;
using System.Diagnostics;
using System.Numerics;
using X.PagedList;

namespace SBWV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? page )
        {
            

            SwapBookDbContext _context = new SwapBookDbContext();
            int pageNumber = page ?? 1;
            int pageSize = 2;
            var books = _context.Books.ToList();
            List<BookVM> bookVMs = new List<BookVM>();
            foreach ( var book in books ) 
            { bookVMs.Add(GetBookModel.GetBookVM(book)); }

            var items = bookVMs.ToPagedList(pageNumber, pageSize);
            return View(items);

        }

        public IActionResult Contacts() 
        {
            return View(); 
        }
        public IActionResult Categories() 
        {
            return View("Categories", new SwapBookDbContext().Catalogs.ToList());
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            

            return View(new ErrorViewModel
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
        
    }
}

