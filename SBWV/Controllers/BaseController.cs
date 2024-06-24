using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SQLitePCL;

namespace SBWV.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsUserLogged()
        {
            return HttpContext.Session.GetInt32("user").HasValue;
        }

        protected int GetUserId()
        {

            return HttpContext.Session.GetInt32("user").Value;

        }


    }
}
