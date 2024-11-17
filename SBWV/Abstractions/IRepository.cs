using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBWV.Models.ViewModels;
using Unidecode.NET;
using X.PagedList;

namespace SBWV.Abstractions
{
    public interface IRepository
    {

        public string AddUser(User user);
       
        public User FindUserLogin(LoginVM login);


        public IEnumerable<BookVM> GetUserBooks(int id);

        public IEnumerable<BookVM> GetBooksByCategory(int idCategory, int? idUser);



        public void DeleteBook(int idBook);


        public BookVM GetBookVM(int idBook);


        public IEnumerable<SelectListItem> GetSelectListCategoryEdit(int idBook);

        public IEnumerable<SelectListItem> GetSelectListCategory();


        public void UpdateBook(Book book);


        public Galary GetGalary(int id);


        public void RemoveGalary(Galary galary);


        public void AddBook(Book book);


        public List<SelectListItem> GetSelectListItemCategory();


        public IEnumerable<BookVM> GetFavoriteBooksVM(int userId);


        public bool AddFavorite(int idUser, int idBook);


        public void DeleteFavorite(int idBook, int idUser);


        public IEnumerable<BookVM> Search(string author);

        public IPagedList<BookVM> Pagination(IEnumerable<BookVM> bookList, int? page);


        public IEnumerable<BookVM> GetSearchResult(string input);

        public IEnumerable<BookVM> GetAllBooks(int? idUser);



        public bool IsEmailComfirmed(string email, string token);


        public User GetUserByEmail(string email);

        public byte[] GetByteArrayOfImage(string idGalary);
           
        
    }
}
