using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBWV.Models;
using SBWV.Models.ViewModels;
using System.Diagnostics;
using System.Numerics;

namespace SBWV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int page = 1)
        {
            List<BookVM> bookVMs = new List<BookVM>();
            using (var db = new SwapBookDbContext())
            {
                var m = db.Books.Include("IdCatalogNavigation")
                     .Include(c => c.Galaries)
                     .ToList();
               

                for (int i = 0; i < m.Count(); i++)
                {
                    bookVMs.Add(new GetBookModel().GetBookVM(m[i]));
                }
            }

            int pageSize = 2; // количество объектов на страницу
            IEnumerable<BookVM> booksPerPages = bookVMs.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = bookVMs.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, BookVMs= booksPerPages };
            return View(ivm);

           
        }

        public IActionResult Contacts() 
        {
            return View(); 
        }
        public IActionResult Categories() 
        {
            return View("Categories", new SwapBookDbContext().Catalogs.ToList());
        }
        // вариант СБ ню 

        //  public IActionResult Index()
        // {
        //     return View("Categories", new SwapBookDbContext().Catalogs.ToList());
        // }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
