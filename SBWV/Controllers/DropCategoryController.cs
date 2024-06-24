using Microsoft.AspNetCore.Mvc;

namespace SBWV.Controllers
{
    public class DropCategoryController : Controller
    {
        public IActionResult _CategoryPartial()
        {
            Catalog catalog = new Catalog();
            return View(catalog.Value);
        }
    }
}
