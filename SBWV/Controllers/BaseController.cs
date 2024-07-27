using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SQLitePCL;

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

            return HttpContext.Session.GetInt32("user").Value;

        }


    }
}
