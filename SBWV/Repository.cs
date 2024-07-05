using SBWV.Controllers;
using SBWV.Models;
using SBWV.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SQLitePCL;
using System.Web.Mvc;

namespace SBWV
{
    public class Repository
    {
        private SwapBookDbContext dbContext;

       
        public Repository(SwapBookDbContext db)
        {
            dbContext = db;
        }

        public void AddUser(User user)
        {
            
            SwapBookDbContext swapBookDbContext = new SwapBookDbContext();

            swapBookDbContext.Add(user);

            swapBookDbContext.SaveChanges();
        }

        public User FindUserLogin(Login login)
        {
            SwapBookDbContext dbContext = new SwapBookDbContext();
            var user = dbContext.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            return user;

        }

        public IEnumerable<BookVM> GetUserBooks(HttpContext httpContext) 
        {
           
            
            List<BookVM> bookVMs = new List<BookVM>();

            var books = dbContext.Books.Where(i => i.IdUser == httpContext.Session.GetInt32("user").Value).Include("IdCatalogNavigation")
                      .Include(c => c.Galaries).ToList();
            for (int i = 0; i < books.Count(); i++)
            {
                bookVMs.Add( GetBookModel.GetBookVM(books[i]));
            }
            return bookVMs;

        }
        public IEnumerable<BookVM> GetBooksByCategory(int idCategory)
        {
            List<BookVM> bookVMs = new List<BookVM>();
            var books = dbContext.Books.Where(i => i.IdCatalog == idCategory).Include("IdCatalogNavigation")
                      .Include(c => c.Galaries).ToList();
            for (int i = 0; i < books.Count(); i++)
            {
                bookVMs.Add(GetBookModel.GetBookVM(books[i]));
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
            }
        }

        public BookVM GetBookVM(int idBook)
        {
            
                var e = dbContext.Books.Include(e => e.Galaries).Include("IdCatalogNavigation").Include("IdUserNavigation").FirstOrDefault(b => b.Id == idBook);

                var book = GetBookModel.GetBookVM(e);
                //

                book.Email = e.IdUserNavigation.Email;

                book.Telephone = e.IdUserNavigation.Phone;

                return  book;
            
        }

        public IEnumerable<SelectListItem>  GetSelectListCategoryEdit(int idBook)
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

            var categories = dbContext.Catalogs.Select(e => new SelectListItem {
                Value = e.Id.ToString(), 
                Text = e.Value }).ToList();
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
            dbContext.Books.Add(book);
            dbContext.SaveChanges();
        }

        public  List<SelectListItem> GetSelectListItemCategory()
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
            var books= dbContext.Books
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
                    IsFavourite = true,
                    Price = x.Price,
                    Swap = x.Swap == 1 ? true : false
                ,
                    Telephone = x.IdUserNavigation.Phone,
                    Title = x.Title,
                    Pictures = GetBookModel.GetBase64Images(x.Galaries).ToArray()
                }).ToList();

            /* var favoriteBooks = dbContext.Favorits
                 .Where(f => f.IdUser == userId)
                 .Join(dbContext.Books, f => f.IdBook, b => b.Id, (f, b) => b)
                 .Include(e => e.Galaries).
                 Select(x => new BookVM
                 {
                     Author = x.Author,
                     Category = x.IdCatalogNavigation.Value,
                     Email = x.IdUserNavigation.Email
                 ,
                     Id = x.Id,
                     Info = x.Info,
                     IsFavourite = true,
                     Price = x.Price,
                     Swap = x.Swap == 1 ? true : false
                 ,
                     Telephone = x.IdUserNavigation.Phone,
                     Title = x.Title,
                     Pictures = GetBookModel.GetBase64Images(x.Galaries).ToArray()
                 }).ToList(); ;*/


            return books;
            
        }

        public void AddFavorite(int idUser , int idBook) 
        {
            var favorite = new Favorit() { IdBook = idBook, IdUser = idUser };
            if (favorite != null)
            {
                // Отсоединить отслеживаемую сущность, чтобы избежать конфликта

                dbContext.Entry(favorite).State = EntityState.Detached;
            }
            dbContext.Favorits.Add(favorite);

            dbContext.SaveChanges();
        }
    }
}
