using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SBWV.Controllers
{

    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error")]
        public IActionResult HandleError()
        {
            // Получаем информацию об исключении, если оно есть
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                var exception = exceptionFeature.Error;
                var path = exceptionFeature.Path;

                // Логируем исключение вместе с путём и стек-трейсом
                _logger.LogError(exception, "Произошла ошибка на пути {Path}", path);
            }
            
            return View();
        }
       
            
    }
}


