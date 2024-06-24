using Microsoft.AspNetCore.Mvc;
using SBWV.Models;
using System.Diagnostics;

namespace SBWV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
