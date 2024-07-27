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
        private readonly Repository Repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(Repository repository, ILogger<HomeController> logger)
        {
            Repository = repository;
            _logger = logger;
        }


        public async Task<IActionResult> Index(string input, int? page)
        {

            /*if (String.IsNullOrEmpty(input))
            {
                if (!new BaseController().IsUserLogged())
                {
                    return View(Repository.Pagination(Repository.GetAllBooks(), page ?? 1));
                }
                else 
                {
                    var favoriteBooks = Repository.GetFavoriteBooksVM(new BaseController().GetUserId());
                    var allBooks = Repository.GetAllBooks();

                    foreach (var item in allBooks)
                    {
                        item.IsFavorite = favoriteBooks.Any(e => e.Id == item.Id) ? true : false;
                    }

                    return View(Repository.Pagination(allBooks, page ?? 1));
                }

            }*/


            if (new BaseController().IsUserLogged())
            {


                var favoriteBooks = Repository.GetFavoriteBooksVM(new BaseController().GetUserId());

                if (!String.IsNullOrEmpty(input))
                {


                    var bookVMs = Repository.GetSearchResult(input);

                    foreach (var item in bookVMs)
                    {
                        item.IsFavorite = favoriteBooks.Any(e => e.Id == item.Id) ? true : false;

                    }

                    ViewBag.input = input;

                    return View("Search", Repository.Pagination(bookVMs, page ?? 1));
                }

                var allBooks = Repository.GetAllBooks();

                foreach (var item in allBooks)
                {
                    item.IsFavorite = favoriteBooks.Any(e => e.Id == item.Id) ? true : false;
                }

                return View(Repository.Pagination(allBooks, page ?? 1));
            }

            if (String.IsNullOrEmpty(input))
            {

                return View(Repository.Pagination(Repository.GetAllBooks(), page ?? 1));
            }

            var items = Repository.Pagination(Repository.GetSearchResult(input), page ?? 1);
            ViewBag.input = input;

            return View("Search", items);


            /* SwapBookDbContext _context = new SwapBookDbContext();
             int pageNumber = page ?? 1;
             int pageSize = 2;
             var books = _context.Books.ToList();
             List<BookVM> bookVMs = new List<BookVM>();

             foreach ( var book in books ) 
             { bookVMs.Add(GetBookModel.GetBookVM(book)); }

             var items = bookVMs.ToPagedList(pageNumber, pageSize);
             return View(items);*/

        }

        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View("Categories", new SwapBookDbContext().Catalogs.ToList());
        }




      /*  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {


            return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });



        }*/
    }
}

