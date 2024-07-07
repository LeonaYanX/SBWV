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
        private Repository repo;
        public AccountController(Repository repository)
        {
          repo = repository;
        }

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
            if (ModelState.IsValid)
            {
                
                    User user = new User
                    {
                        Age = register.Age,
                        City = register.City,
                        Email = register.Email,
                        Password = register.Password,
                        Phone = register.Phone
                    };

                    repo.AddUser(user);

                    HttpContext.Session.SetString("email", user.Email ?? "Not Specified");

                    HttpContext.Session.SetInt32("user", user.Id);

                    return RedirectToAction("Index", "Home");
                
               
            }
            else 
            {
                return RedirectToAction("Register", "Account");
            }
           

           
        }
        [HttpPost]
        public IActionResult Login(Login login)
        {
            SwapBookDbContext dbContext = new SwapBookDbContext();

            var user = repo.FindUserLogin(login);

            // todo remove if right
            //var user = dbContext.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("email", user.Email);

                HttpContext.Session.SetInt32("user", user.Id);

                return RedirectToAction("Info", "Account");
            }
            else
            {
                return View(); // todo view for accsess denied reenter red line 
            }

        }

        public IActionResult Info()
        {
            // Возвращение  списка книг- обьявлений юзера

          //  SwapBookDbContext swapBookDbContext = new SwapBookDbContext();

         //   List<BookVM> bookVMs = new List<BookVM>();

            if (IsUserLogged())
            {
               // var books = swapBookDbContext.Books.Where(i => i.IdUser == GetUserId()).Include("IdCatalogNavigation")
               //        .Include(c => c.Galaries).ToList();
               // for (int i = 0; i < books.Count(); i++)
                //{
                 //   bookVMs.Add(new GetBookModel().GetBookVM(books[i]));
               // }

               // return View(bookVMs);
               return View(repo.GetUserBooks(HttpContext));
            }

            else
                return RedirectToAction("Login");
        }

        public IActionResult Favorites() 
        {
            if (IsUserLogged())
            {
                var userId = GetUserId();


                return View(repo.GetFavoriteBooksVM(userId));
            }
            else 
            {
               return RedirectToAction("Login");
            }
        
        }
        public IActionResult DeleteFavorites(int idBook)
        {
            if (IsUserLogged()) 
            {
            var userId = GetUserId();
                repo.DeleteFavorite(idBook , userId);
                return RedirectToAction("Favorites", "Account");
}
            else
            {
                return RedirectToAction("Login");
            }
           
           
         
        }

        public JsonResult AddFavorites(int idBook)
        {
            if (IsUserLogged())
            {
                try
                {
                    var userId = GetUserId();
                    repo.AddFavorite(userId, idBook);
                    return Json(new { success = true, msg = "Книга добавлена в избранное" });
                }

                
                catch (Exception ex) 
                {
                   return Json(new { success = false, msg = ex.Message });
                }
                
                
            }
            else 
            {
                return Json(new {success= false , msg="Вы не авторизированны" });
            }
        }

    }
}
