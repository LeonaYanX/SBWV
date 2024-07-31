using Microsoft.AspNetCore.Mvc;



namespace SBWV.Controllers
{

    public class SearchController : Controller
    {
        private Repository repo;

        

        public SearchController(Repository repository)
        {
            repo = repository;

            
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







