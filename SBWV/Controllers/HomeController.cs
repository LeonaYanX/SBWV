using Microsoft.AspNetCore.Mvc;

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




        }

        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View("Categories", new SwapBookDbContext().Catalogs.ToList());
        }





    }
}

