using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        // Handle GET /Store/Cart > show a list of items in our cart
        [Authorize]
        public IActionResult Cart()
        {
            string customerId = User.Identity.Name; // username = email address

            // query list of products in cart using LINQ
            var cart = _context.Carts
                        .Include(c => c.Product)
                        .Where(c => c.CustomerId == customerId)
                        .ToList();

            // calculate total and pass as viewbag field
            // use sum aggregation function to sum all prices in the list
            var total = cart.Sum(c => c.Price);
            ViewBag.TotalAmount = total.ToString("C");

            return View(cart);            
        }

        // API endpoint to add an item to the cart
        [Authorize]
        public IActionResult AddToCart(int ProductId, int Quantity)
        { 
            // get product price
            var price = _context.Products.Find(ProductId).Price;

            // get customerid
            var customerId = User.Identity.Name;

            // create and save cart object
            // cart is an item in the shopping cart
            var cart = new Cart();
            cart.ProductId = ProductId;
            cart.Quantity = Quantity;
            cart.Price = price * Quantity; // price of individual product times quantity
            cart.DateCreated = System.DateTime.UtcNow; // Always save datetime in utc format
            cart.CustomerId = customerId;

            _context.Carts.Add(cart);
            _context.SaveChanges();

            //redirect to cart view
            return Redirect("Cart");
            
        }

        // API endpoint to remove and item from the cart

        [Authorize]
        public IActionResult RemoveFromCart(int id) {
            var cartItem = _context.Carts.Where(c => c.Id == id).FirstOrDefault();

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

    }
}
