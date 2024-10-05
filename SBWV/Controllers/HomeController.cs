using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SBWV.Abstractions;


namespace SBWV.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRepository _repository;
        private readonly ILogger<HomeController> _logger;


        public HomeController(IRepository repository, ILogger<HomeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        
        public async Task<IActionResult> Index(string input, int? page)
        {

            if (IsUserLogged())
            {

                var favoriteBooks = _repository.GetFavoriteBooksVM(GetUserId());

                if (!String.IsNullOrEmpty(input))
                {


                    var bookVMs = _repository.GetSearchResult(input).ToList(); // todo kak?

                    foreach (var item in bookVMs)
                    {
                        var isFavorite = favoriteBooks.Any(e => e.Id == item.Id)? true : false;
                        item.IsFavorite = isFavorite;
                    }

                    ViewBag.input = input;

                    return View("Search", _repository.Pagination(bookVMs, page ?? 1));
                }
                else 
                {
                    var allBooks = _repository.GetAllBooks(GetUserId());

                    return View(_repository.Pagination(allBooks, page ?? 1));
                }
               
            }
            else 
            {
                if (String.IsNullOrEmpty(input))
                {

                    return View(_repository.Pagination(_repository.GetAllBooks(null), page ?? 1));
                }

                var items = _repository.Pagination(_repository.GetSearchResult(input), page ?? 1);
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
            
            byte[] image = _repository.GetByteArrayOfImage(idGalary);

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

