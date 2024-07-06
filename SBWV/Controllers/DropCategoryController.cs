using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace SBWV.Controllers
{

    public class DropCategoryController : Controller
    {
     

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
      
        
       
    

   

