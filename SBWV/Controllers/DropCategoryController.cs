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

        public IActionResult Search(string input)
        {
            // var bookList = repo.Search(author);
            SwapBookDbContext swapBookDbContext = new SwapBookDbContext();
            if (String.IsNullOrEmpty(input))
                return View( swapBookDbContext.Books.Select(b=>GetBookModel.GetBookVM(b)).ToList());

            input = input.ToLower();

            var bookListCategory = swapBookDbContext.Books.Where(b => b.IdCatalogNavigation.Value.ToLower().Contains(input)).ToList();
            var bookListTitle = swapBookDbContext.Books.Where(b => b.Title.ToLower().Contains(input)).ToList();
            var bookListAuthor = swapBookDbContext.Books.Where(b => b.Author.ToLower().Contains(input)).ToList();

            bookListCategory.AddRange(bookListTitle);
            bookListCategory.AddRange(bookListAuthor);

            var bookList = bookListCategory.Distinct().Select(w=>GetBookModel.GetBookVM(w));

            
            return View( bookList);
        }
    }
}
      
        
       
    

   

