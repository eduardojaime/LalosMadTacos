using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    // Handles requests to /Store
    public class StoreController : Controller
    {
        // Handles /Store/Index or /Store > Shows a list of categories
        public IActionResult Index()
        {
            return View(); // returns /views/store/index.cshtml
        }

        // Handles /Store/Browse > Shows a list of menu items filtered by category
        public IActionResult Browse()
        {
            return View(); // returns /views/store/browse.cshtml
        }

        // Handle /Store/Details > Shows details of a menu item
        public IActionResult Details()
        {
            return View(); // return /views/store/details.cshtml
        }
    }
}
