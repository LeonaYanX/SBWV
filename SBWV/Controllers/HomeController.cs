using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace SBWV.Controllers
{
    public class HomeController : BaseController
    {
        private readonly Repository Repository;
        private readonly ILogger<HomeController> _logger;


        public HomeController(Repository repository, ILogger<HomeController> logger)
        {
            Repository = repository;
            _logger = logger;
        }

        
        public async Task<IActionResult> Index(string input, int? page)
        {

            if (IsUserLogged())
            {

                var favoriteBooks = Repository.GetFavoriteBooksVM(GetUserId());

                if (!String.IsNullOrEmpty(input))
                {


                    var bookVMs = Repository.GetSearchResult(input).ToList(); // todo kak?

                    foreach (var item in bookVMs)
                    {
                        var isFavorite = favoriteBooks.Any(e => e.Id == item.Id)? true : false;
                        item.IsFavorite = isFavorite;
                    }

                    ViewBag.input = input;

                    return View("Search", Repository.Pagination(bookVMs, page ?? 1));
                }
                else 
                {
                    var allBooks = Repository.GetAllBooks(GetUserId());

                    return View(Repository.Pagination(allBooks, page ?? 1));
                }
               
            }
            else 
            {
                if (String.IsNullOrEmpty(input))
                {

                    return View(Repository.Pagination(Repository.GetAllBooks(null), page ?? 1));
                }

                var items = Repository.Pagination(Repository.GetSearchResult(input), page ?? 1);
                ViewBag.input = input;

                return View("Search", items);
            }

        }

        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View("Categories", new SwapBookDbContext().Catalogs.ToList());
        }


        public IActionResult GetImageById(string idGalary)
        {
            
            byte[] image = Repository.GetByteArrayOfImage(idGalary);

            if (image != null)
            {
                return File(image, "image/png");
            }

            else 
            {
            return  NotFound();
            }
           
        }


    }
}

