using Microsoft.AspNetCore.Mvc;

namespace SBWV.Controllers
{
    public class BaseController : Controller
    {
        public bool IsUserLogged()
        {
            return HttpContext?.Session?.GetInt32("user").HasValue ?? false;
        }

        public int GetUserId()
        {
            //todo null reference exception
            return HttpContext.Session.GetInt32("user").Value;


        }


    }
}
