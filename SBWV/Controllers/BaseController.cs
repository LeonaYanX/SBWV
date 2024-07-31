using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SBWV.Controllers
{
    public class BaseController : Controller
    {
        public bool IsUserLogged()
        {

            return HttpContext.User.Identity.IsAuthenticated;
        }

        public int GetUserId()
        {

            return int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

         
        }

        public void SignInUser(string email , int userId)
        {

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, email));
            claims.Add(new Claim("Id", userId.ToString()));
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }


    }
}
