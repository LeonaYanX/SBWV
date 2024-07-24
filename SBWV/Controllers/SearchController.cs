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

        public async Task<IActionResult> Search(string input )
        {
            // var bookList = repo.Search(author);
            SwapBookDbContext swapBookDbContext = new SwapBookDbContext();
            if (String.IsNullOrEmpty(input))
                return View( swapBookDbContext.Books.Select(b=>GetBookModel.GetBookVM(b)).ToList());
           /* TempData["input"] = input;*/
            
            /*input = input.ToLower();

            var trasliteratedInputFromRussian =  input.Unidecode();
           //var transFromEnglish = transliteration.TransliterateENToRU(input);
            var allBooks = swapBookDbContext.Books.ToList();


          //  var bookListCategory = swapBookDbContext.Books.Where(p => EF.Functions.Like(p.IdCatalogNavigation.Value.ToLower(), "%" + input + "%")).ToList();
            var bookListTitle = swapBookDbContext.Books.Where(b => b.TitleLC.Contains(input) 
            || b.TitleLC.Contains(trasliteratedInputFromRussian)
            ).ToList();
            var bookListAuthor = allBooks.Where(p => p.AuthorLC.Contains( input ) 
            || p.AuthorLC.Contains(trasliteratedInputFromRussian)
            ).ToList();

            bookListTitle.AddRange(bookListTitle);
            bookListTitle.AddRange(bookListAuthor);

            var bookList = bookListTitle.Distinct().Select(w=>GetBookModel.GetBookVM(w));*/

           var bookList = repo.GetSearchResult(input);
            var page = (TempData["page"] as int?)?? 1;
            var items = repo.Pagination(bookList, page);
            
            return View( items);
        }

        public void SearchPagination(int ?page)
        {
            TempData["page"]= page;
            /*var input = (TempData["input"] as string) ?? "";*/
          // var items = repo.Pagination(repo.GetSearchResult(input) , page);

            return;
        }

       
    }
}
      
        
       
    

   

