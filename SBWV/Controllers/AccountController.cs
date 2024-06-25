using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using SBWV.Models;
using SBWV.Controllers;
using Microsoft.EntityFrameworkCore;
using SBWV.Models.ViewModels;

namespace SBWV.Controllers
{


    public class AccountController : BaseController
    {

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(Register register)
        {
            // todo try{} catch{} unique email
            try 
            {
                User user = new User
                {
                    Age = register.Age,
                    City = register.City,
                    Email = register.Email,
                    Password = register.Password,
                    Phone = register.Phone
                };
                SwapBookDbContext swapBookDbContext = new SwapBookDbContext();

                swapBookDbContext.Add(user);

                swapBookDbContext.SaveChanges();

                HttpContext.Session.SetString("email", user.Email ?? "Not Specified");

                HttpContext.Session.SetInt32("user", user.Id);

                return RedirectToAction("Index", "Home");
            }
            catch(Exception)
            {
                return RedirectToAction("Register", "Account");
            }
           

           
        }
        [HttpPost]
        public IActionResult Login(Login login)
        {
            SwapBookDbContext dbContext = new SwapBookDbContext();

            var user = dbContext.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("email", user.Email);

                HttpContext.Session.SetInt32("user", user.Id);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(); // todo view for accsess denied reenter red line 
            }

        }

        public IActionResult Info()
        {
            // Возвращение  списка книг- обьявлений юзера

            SwapBookDbContext swapBookDbContext = new SwapBookDbContext();

            List<BookVM> bookVMs = new List<BookVM>();

            if (IsUserLogged())
            {
                var books = swapBookDbContext.Books.Where(i => i.IdUser == GetUserId()).Include("IdCatalogNavigation")
                       .Include(c => c.Galaries).ToList();
                for (int i = 0; i < books.Count(); i++)
                {
                    bookVMs.Add(new GetBookModel().GetBookVM(books[i]));
                }

                return View(bookVMs);
            }

            else
                return RedirectToAction("Login");
        }

    }
}
