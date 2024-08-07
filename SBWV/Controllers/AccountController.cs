using Microsoft.AspNetCore.Mvc;
using SBWV.Models;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SBWV.Service;

namespace SBWV.Controllers
{



    public class AccountController : BaseController
    {
        private Repository repo;

        private MailSender mailSender;
        public AccountController(Repository repository , MailSender mailSender)
        {
            this.mailSender = mailSender;
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
                    Phone = register.Phone,
                    Token = Guid.NewGuid().ToString()
                };



               string result= repo.AddUser(user);

                if (!String.IsNullOrEmpty(result)) 
                {
                   TempData["Message"] = result;

                    return RedirectToAction("Register", "Account");
                }


               // SignInUser(user.Email, user.Id);

                mailSender.SendEmailConfirmation(user.Email , user.Token);

                TempData["Message"] = "На указанную элекронный адрес вам отправленно письмо для подтверждения почты.";

                return RedirectToAction("Login", "Home");


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
                
                    TempData["Message"] = "Пользователь с такой почтой или паролем не найден";
                    return  View("Login"); //Unauthorized();

            }

            else if (user.IsComfirmed)
            {
                SignInUser(user.Email, user.Id);

                return RedirectToAction("Info", "Account");
            }

            else 
            {
                TempData["Message"] = "Почта не подтверждена";
                return View("Login");
            }

            
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

        public IActionResult ComfirmEmail(string email, string token) 
        {
            if (repo.IsEmailComfirmed(email, token))
            {
                TempData["Message"] = "Вы подтвердили почту";
                var user = repo.GetUserByEmail(email);
                SignInUser(user.Email, user.Id);
                return RedirectToAction("Info");
            }
            else 
            {
                TempData["Message"] = "Вы не подтвердили почту";

                return View("Login");
            }
            
        }

    }
}
