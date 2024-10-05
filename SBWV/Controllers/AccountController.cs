using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SBWV.Service;
using SBWV.Models.ViewModels;
using SBWV.Abstractions;

namespace SBWV.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IRepository _repo;

        private readonly MailSender _mailSender;
        public AccountController(IRepository repository , MailSender mailSender)
        {
            _mailSender = mailSender;
            _repo = repository;
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
        public IActionResult Register(RegisterVM register) 
        {
            if (ModelState.IsValid) // проверка валидации
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



               string result= _repo.AddUser(user);

                if (!String.IsNullOrEmpty(result)) 
                {
                   TempData["Message"] = result;

                    return RedirectToAction("Register", "Account");
                }


               // SignInUser(user.Email, user.Id);

                _mailSender.SendEmailConfirmation(user.Email , user.Token);

                TempData["Message"] = "На указанную элекронный адрес вам отправленно письмо для подтверждения почты.";

                return RedirectToAction("Login", "Home");


            }
            else
            {
                return RedirectToAction("Register", "Account");
            }



        }
        


        [HttpPost]
        public IActionResult Login(LoginVM login)
        {

            var user = _repo.FindUserLogin(login);

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

                return View(_repo.GetUserBooks(GetUserId()));
            

            
        }
        [Authorize]
        public IActionResult Favorites()
        {
                var userId = GetUserId();


                return View(_repo.GetFavoriteBooksVM(userId));
        }
        public IActionResult DeleteFavorites(int idBook)
        {
           
                var userId = GetUserId();
                _repo.DeleteFavorite(idBook, userId);
                return RedirectToAction("Favorites", "Account");



        }
        [Authorize]
        public JsonResult AddFavorites(int idBook)
        {
                try
                { 
                    var userId = GetUserId();

                    var isAdded = _repo.AddFavorite(userId, idBook);

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
            if (_repo.IsEmailComfirmed(email, token))
            {
                TempData["Message"] = "Вы подтвердили почту";
                var user = _repo.GetUserByEmail(email);
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
