using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SBWV.Controllers
{

    public class DropCategoryController : Controller
    {
        private Repository repo;

        public DropCategoryController(Repository repository)
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

        public IActionResult Search(string author) 
        {
             var bookList = repo.Search(author);

            return View("BooksList", bookList);
        }
    }
}
      
        
       
    

   

