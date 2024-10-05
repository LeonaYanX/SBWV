using Microsoft.AspNetCore.Mvc;
using SBWV.Abstractions;



namespace SBWV.Controllers
{

    public class SearchController : Controller
    {
        private readonly IRepository _repo;

        

        public SearchController(IRepository repository)
        {
            _repo = repository;

            
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







