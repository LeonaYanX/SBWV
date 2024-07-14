using Microsoft.AspNetCore.Mvc;

namespace SBWV.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult HandleError()
        {
            // Логика обработки ошибок
            return View();
        }
    }
}
