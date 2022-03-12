using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApp.Extensions;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    // Handles requests to /Store
    public class StoreController : Controller
    {
        // Configure dependency injection to access the db context object
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public StoreController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        public IActionResult RemoveFromCart(int id)
        {
            var cartItem = _context.Carts.Where(c => c.Id == id).FirstOrDefault();

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        // checkout route to handle payment setup for items in the shopping cart
        [Authorize]
        public IActionResult Checkout()
        {
            // create temp order to pass total
            var customerId = User.Identity.Name;
            Models.Order tempOrder = new Models.Order();
            var cartItems = _context.Carts.Where(c => c.CustomerId == customerId).ToList();
            tempOrder.Total = cartItems.Sum(c => c.Price);

            return View(tempOrder);
        }

        // Post Handler > handles when user clicks Submit button on checkout page
        [Authorize] // only authenticated users can checkout
        [ValidateAntiForgeryToken] // adds protection so that this action cannot be hijacked
        [HttpPost] // gets triggered when customer clicks submit button on checkout page
        public IActionResult Checkout([Bind("DeliveryNotes")] Models.Order order)
        {
            // Order gets created in the View            
            // recalculate total
            var customerId = User.Identity.Name;
            var cartItems = _context.Carts.Where(c => c.CustomerId == customerId).ToList();
            order.Total = cartItems.Sum(c => c.Price);
            order.OrderDate = System.DateTime.UtcNow; // ALWAYS STORE DATE VALUES IN UTC time
            order.CustomerId = customerId;

            // save order object in session so we can reuse it later
            // create SessionsExtension class:
            // https://www.talkingdotnet.com/store-complex-objects-in-asp-net-core-session/
            HttpContext.Session.SetObject("Order", order);

            // redirect to payment
            return RedirectToAction("Payment");
        }

        // payments route to actually handle payments by calling the stripe API
        [Authorize]
        public IActionResult Payment()
        {
            // get order object from session store
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            // calculate total
            ViewBag.Total = order.Total * 100;
            // get publishable key from appsettings.json
            ViewBag.PublishableKey = _configuration["Stripe:PublishableKey"];

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Payment(string stripeToken)
        {
            // get order from session store
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            // Import Stripe and Stripe.Checkout in your program
            // Fix any reference to Order class, declare it explicitly: Models.Order
            // get the stripe configuration key
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            // configure stripe: create payment session
            // Create Stripe session object >> https://stripe.com/docs/checkout/integration-builder
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long?)(order.Total * 100), // amount in cents
                            Currency = "cad",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "LalosMadTacos Purchase"
                            }
                        },
                        Quantity =1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"https://{Request.Host}/Store/SaveOrder",
                CancelUrl = $"https://{Request.Host}/Store/Cart"
            };

            // return session id for JavaScript code to use in calling stripe
            var service = new SessionService(); // A service is... a class that produces something

            // now use the service object to create a session object based on the options
            // what this does is calls Stripe's API to create a session on their end
            // we have the ID value which will be used to redirect the user to Stripe
            // With this ID stripe will load the amount information on their end
            Session session = service.Create(options);

            // return json response
            return Json(new { id = session.Id });
        }

        [Authorize]
        public IActionResult SaveOrder()
        {
            // this is triggered post payment (after user entered CC details on Stripe portal)
            // retrieve order from session store
            var order = HttpContext.Session.GetObject<Models.Order>("Order");

            // save order in DB
            _context.Orders.Add(order);
            _context.SaveChanges();

            // clear cart
            var customerId = User.Identity.Name;
            var cartItems = _context.Carts.Where(c => c.CustomerId == customerId).ToList();
            // loop through item list and remove them from _context.Carts
            foreach (var cartItem in cartItems)
            {
                // cartItem becomes an orderDetail
                // use cartItem object to fill in a new orderDetail object
                var orderDetail = new OrderDetail();
                orderDetail.OrderId = order.Id;
                orderDetail.ProductId = cartItem.ProductId;
                orderDetail.Quantity = cartItem.Quantity;
                orderDetail.Price = cartItem.Price;

                // save orderDetail record in db
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();

                // remove the cart
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }

            // redirect to order details view
            // /Orders/Details/@id
            return RedirectToAction("Details", "Orders", new { @id = order.Id });
        }

    }
}
