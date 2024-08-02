using Microsoft.AspNetCore.Mvc;
using SBWV.Models;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        public async Task<IActionResult>  Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 
           
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



               string result= repo.AddUser(user);

                if (!String.IsNullOrEmpty(result)) 
                {
                   TempData["Message"] = result;

                    return RedirectToAction("Register", "Account");
                }

                SignInUser(user.Email, user.Id);

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

            var user = repo.FindUserLogin(login);

            if (user == null)
            {
                return Unauthorized();
            }

            SignInUser(user.Email, user.Id);

            return RedirectToAction("Info", "Account");
        }
        [Authorize]
        public IActionResult Info()
        {
            // Возвращение  списка книг- обьявлений юзера

                return View(repo.GetUserBooks(GetUserId()));
            

            
        }
        [Authorize]
        public IActionResult Favorites()
        {
                var userId = GetUserId();


                return View(repo.GetFavoriteBooksVM(userId));
        }
        public IActionResult DeleteFavorites(int idBook)
        {
           
                var userId = GetUserId();
                repo.DeleteFavorite(idBook, userId);
                return RedirectToAction("Favorites", "Account");



        }
        [Authorize]
        public JsonResult AddFavorites(int idBook)
        {
                try
                { 
                    var userId = GetUserId();

                    var isAdded = repo.AddFavorite(userId, idBook);

                    return Json(new { success = true, msg = isAdded ? "Книга добавлена в избранное" : "Книга удалена из избранного", isAdded = isAdded });
                }


                catch (Exception ex)
                {
                    return Json(new { success = false, msg = ex.Message, isAdded = false });
                }


           //????
           /* else
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Json(new { success = false, msg = "Вы не авторизированны" });
            }*/
        }

    }
}
