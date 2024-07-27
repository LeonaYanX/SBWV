using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Unidecode;
using Unidecode.NET;
using SBWV.Service;
using SBWV.Models.ViewModels;
using X.PagedList;


namespace SBWV.Controllers
{

    public class SearchController : Controller
    {
        private Repository repo;

        private Transliteration transliteration;

        public SearchController(Repository repository , Transliteration transliteration)
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

       /* public async Task<IActionResult> Search(string input, int? page)
        {
            if (String.IsNullOrEmpty(input))
            {
                SwapBookDbContext dbContext = new SwapBookDbContext();
                return View(repo.Pagination(dbContext.Books.Select(e => GetBookModel.GetBookVM(e)), page ?? 1));
            }

            var items = repo.Pagination(repo.GetSearchResult(input), page ?? 1);
            ViewBag.input = input;

            return View(items);
        }*/

       

       
    }
}
      
        
       
    

   

