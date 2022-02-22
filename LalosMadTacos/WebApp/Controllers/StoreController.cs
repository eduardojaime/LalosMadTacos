using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using System.Linq;

namespace WebApp.Controllers
{
    // Handles requests to /Store
    public class StoreController : Controller
    {
        // Configure dependency injection to access the db context object
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Handles GET /Store/Index or /Store > Shows a list of categories
        public IActionResult Index()
        {
            // Import System.Linq namespace to access Linq methods such as OrderBy
            var categoryList = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(categoryList); // returns /views/store/index.cshtml
        }

        // Handles GET /Store/Browse/{id} > Shows a list of menu items filtered by category
        public IActionResult Browse(int id) // automatically map /{id} with id
        {
            // Use context object to query the database and get a list of products by categoryId
            // Use LINQ
            // https://www.tutorialsteacher.com/linq/what-is-linq
            // https://linqsamples.com/linq-to-objects/aggregation
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
            // vs SQL >> string query = "SELECT * FROM PRODUCTS WHERE CATEGORYID = @CATID";
            var menuItems = _context.Products
                .Where(p => p.CategoryId == id)
                .OrderBy(p => p.Name)
                .ToList();

            return View(menuItems); // returns /views/store/browse.cshtml
        }

        // Handle GET /Store/Details > Shows details of a menu item
        public IActionResult Details()
        {
            return View(); // return /views/store/details.cshtml
        }
    }
}
