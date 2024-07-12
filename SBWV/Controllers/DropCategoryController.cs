using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


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

           
            var allBooks = swapBookDbContext.Books.ToList();


            var bookListCategory = swapBookDbContext.Books.Where(p => EF.Functions.Like(p.IdCatalogNavigation.Value.ToLower(), "%" + input + "%")).ToList();
            var bookListTitle = swapBookDbContext.Books.Where(b => b.Title.ToLower().IndexOf(input) >= 0).ToList();
            var bookListAuthor = allBooks.Where(p => p.Author.ToLower().Contains( input )).ToList();

            bookListCategory.AddRange(bookListTitle);
            bookListCategory.AddRange(bookListAuthor);

            var bookList = bookListCategory.Distinct().Select(w=>GetBookModel.GetBookVM(w));

            
            return View( bookList);
        }
    }
}
      
        
       
    

   

