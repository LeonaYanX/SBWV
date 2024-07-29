using Microsoft.AspNetCore.Mvc;

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

            //todo null reference exception
           // return HttpContext.Session.GetInt32("user").Value;


        }


    }
}
