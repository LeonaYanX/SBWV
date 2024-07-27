using Microsoft.AspNetCore.Mvc;
using SBWV.Service;


namespace SBWV.Controllers
{

    public class SearchController : Controller
    {
        private Repository repo;

        private Transliteration transliteration;

        public SearchController(Repository repository, Transliteration transliteration)
        {
            repo = repository;

            this.transliteration = transliteration;
        }


        public IActionResult GetCategories()
        {

            return View("_GetCategoriesPartial");
        }

        public IActionResult GetAuthors()
        {

            return View("_GetAuthorsPartial");
        }


    }
}







