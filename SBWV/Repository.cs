using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBWV.Controllers;
using SBWV.Models;
using SBWV.Models.ViewModels;
using Unidecode.NET;
using X.PagedList;
using static System.Reflection.Metadata.BlobBuilder;


namespace SBWV
{
    public class Repository
    {


        private SwapBookDbContext dbContext;


        public Repository(SwapBookDbContext db)
        {
            dbContext = db;
        }

        public string AddUser(User user)
        {
            try
            {
                dbContext.Add(user);
                dbContext.SaveChanges();
            }

            catch (Exception) 
            {
             return "Такой пользователь уже существует";
            }
            return String.Empty;
            
        }

        public User FindUserLogin(Login login)
        {
            SwapBookDbContext dbContext = new SwapBookDbContext();
            var user = dbContext.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            return user;

        }

        public IEnumerable<BookVM> GetUserBooks(int id)
        {


            List<BookVM> bookVMs = new List<BookVM>();

            var books = dbContext.Books.Where(i => i.IdUser == id).Include("IdCatalogNavigation")
                      .Include(c => c.Galaries).ToList();
            for (int i = 0; i < books.Count(); i++)
            {
                bookVMs.Add(GetBookModel.GetBookVM(books[i]));
            }
            return bookVMs;

        }
        public IEnumerable<BookVM> GetBooksByCategory(int idCategory, int? idUser)
        {
            List<Favorit> favorites = new List<Favorit>();

            if (idUser != null)
            {
                favorites = dbContext.Favorits.Where(f => f.IdUser == idUser).ToList();

            }
            List<BookVM> bookVMs = new List<BookVM>();
            var books = dbContext.Books.Where(i => i.IdCatalog == idCategory).Include("IdCatalogNavigation")
                      .Include(c => c.Galaries).ToList();
            for (int i = 0; i < books.Count(); i++)
            {
                var book = GetBookModel.GetBookVM(books[i]);
                book.IsFavorite = favorites.Any(f => f.IdBook == books[i].Id);
                bookVMs.Add(book);

            }
            return bookVMs;
        }

     
        public void DeleteBook(int idBook)
        {
            var bookToDelete = dbContext.Books.FirstOrDefault(e => e.Id == idBook);
            if (bookToDelete != null)
            {
                dbContext.Books.Remove(bookToDelete);
                dbContext.SaveChanges();
                // while saving changes SqliteException: SQLite Error 19: 'FOREIGN KEY constraint failed'.

            }
        }

        public BookVM GetBookVM(int idBook)
        {

            var e = dbContext.Books.Include(e => e.Galaries).Include("IdCatalogNavigation").Include("IdUserNavigation").FirstOrDefault(b => b.Id == idBook);

            var book = GetBookModel.GetBookVM(e);
            // todo null reference exception

            book.Email = e.IdUserNavigation.Email;

            book.Telephone = e.IdUserNavigation.Phone;

            return book;

        }

        public IEnumerable<SelectListItem> GetSelectListCategoryEdit(int idBook)
        {
            var e = dbContext.Books.Include(e => e.Galaries).Include("IdCatalogNavigation").Include("IdUserNavigation").FirstOrDefault(b => b.Id == idBook);
            List<SelectListItem> categories = dbContext.Catalogs.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Value
                     ,
                Selected = e.IdCatalog == c.Id
            }).ToList();
            return categories;
        }

        public IEnumerable<SelectListItem> GetSelectListCategory()
        {

            var categories = dbContext.Catalogs.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Value
            }).ToList();
            categories.First().Selected = true;

            return categories;
        }

        public void UpdateBook(Book book)
        {
            var trackedEntity = dbContext.Books.Local.FirstOrDefault(b => b.Id == book.Id);

            if (trackedEntity != null)
            {
                // Отсоединить отслеживаемую сущность, чтобы избежать конфликта

                dbContext.Entry(trackedEntity).State = EntityState.Detached;
            }
            book.AuthorLC = (book.Author).ToLower();
            book.TitleLC = (book.Title).ToLower();



            dbContext.Books.Update(book);
            dbContext.SaveChanges();

        }

        public Galary GetGalary(int id)
        {
            var galary = dbContext.Galaries.Find(id);
            return galary;
        }

        public void RemoveGalary(Galary galary)
        {
            dbContext.Galaries.Remove(galary);
            dbContext.SaveChanges();
        }

        public void AddBook(Book book)
        {
            book.AuthorLC = (book.Author).ToLower();
            book.TitleLC = (book.Title).ToLower();

            dbContext.Books.Add(book);
            dbContext.SaveChanges();
        }

        public List<SelectListItem> GetSelectListItemCategory()
        {

            var genreList = dbContext.Catalogs.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Value,

            }).ToList();
            return genreList;
        }

        public IEnumerable<BookVM> GetFavoriteBooksVM(int userId)
        {
            var favoriteBooks = dbContext.Favorits.Where(f => f.IdUser == userId);
            var books = dbContext.Books
                .Where(b => favoriteBooks.Any(f => f.IdBook == b.Id))
                .Include(e => e.Galaries).
                Select(x => new BookVM
                {
                    Author = x.Author,
                    Category = x.IdCatalogNavigation.Value,
                    Email = x.IdUserNavigation.Email
                ,
                    Id = x.Id,
                    Info = x.Info,
                    IsFavorite = true,
                    Price = x.Price,
                    Swap = x.Swap == 1 ? true : false
                ,
                    Telephone = x.IdUserNavigation.Phone,
                    Title = x.Title,
                    Pictures = GetBookModel.GetBase64Images(x.Galaries).ToArray()
                }).ToList();




            return books;

        }

        public bool AddFavorite(int idUser, int idBook)
        {
            bool result;
            if (dbContext.Favorits.Any(f => f.IdBook == idBook && f.IdUser == idUser))
            {
                dbContext.Favorits.Remove(dbContext.Favorits.FirstOrDefault(f => f.IdBook == idBook && f.IdUser == idUser));
                result = false;
            }
            else
            {
                var favorite = new Favorit() { IdBook = idBook, IdUser = idUser };

                dbContext.Favorits.Add(favorite);

                result = true;

            }

            dbContext.SaveChanges();

            return result;




        }

        public void DeleteFavorite(int idBook, int idUser)
        {
            var bookToDelete = dbContext.Favorits.FirstOrDefault(x => x.IdBook == idBook && x.IdUser == idUser);
            if (bookToDelete != null)
                dbContext.Favorits.Remove(bookToDelete);
            dbContext.SaveChanges();
        }

        public IEnumerable<BookVM> Search(string author)
        {
            var books = dbContext.Books.Where(x => x.Author == author).Select(y => GetBookVM(y.Id));


            return books;

        }

        public IPagedList<BookVM> Pagination(IEnumerable<BookVM> bookList, int? page)
        {

            int pageNumber = page ?? 1;
            int pageSize = 2;

            var items = bookList.ToPagedList(pageNumber, pageSize);

            return items;
        }

        public IEnumerable<BookVM> GetSearchResult(string input)
        {
            SwapBookDbContext swapBookDbContext = new SwapBookDbContext();
            input = input.ToLower();

            var trasliteratedInputFromRussian = input.Unidecode();

            var allBooks = swapBookDbContext.Books.ToList();

            var bookListTitle = swapBookDbContext.Books.Where(b => b.TitleLC.Contains(input)
            || b.TitleLC.Contains(trasliteratedInputFromRussian)
            ).ToList();
            var bookListAuthor = allBooks.Where(p => p.AuthorLC.Contains(input)
            || p.AuthorLC.Contains(trasliteratedInputFromRussian)
            ).ToList();

            bookListTitle.AddRange(bookListTitle);
            bookListTitle.AddRange(bookListAuthor);

            var bookList = bookListTitle.Distinct().Select(w => GetBookModel.GetBookVM(w));

            return bookList;
        }

        public IEnumerable<BookVM> GetAllBooks(int ?idUser )
        {
            var books = dbContext.Books.Select(e => e).ToList();

            List<BookVM> bookVMs = new List<BookVM>();

            if (idUser != null) 
            {
                var favorites = dbContext.Favorits.Where(f => f.IdUser == idUser).ToList();

                
                for (int i = 0; i < books.Count(); i++)
                {
                    var book = GetBookModel.GetBookVM(books[i]);
                    book.IsFavorite = favorites.Any(f => f.IdBook == books[i].Id);
                    bookVMs.Add(book);

                }

                return bookVMs;
            }

            else 
            {
                foreach (var item in books)
                {
                    bookVMs.Add(GetBookModel.GetBookVM(item));
                }

               
            }

            return bookVMs;
        }

        
    }
}
