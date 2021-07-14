using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LalosMadTacos.Data;
using LalosMadTacos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace LalosMadTacos.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCart
        public IActionResult Index()
        {
            string CustomerId = GetCustomerId();

            // for now just get id == 100
            ShoppingCart shoppingCart = _context.ShoppingCarts
                                        .Where(s => s.CustomerId == CustomerId)
                                        .FirstOrDefault();
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.CustomerId = CustomerId;
                shoppingCart.Created = DateTime.UtcNow;
                shoppingCart.IsActive = true;
                shoppingCart.Items = new List<MenuItem>();

                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }

            return View(shoppingCart);
        }

        public RedirectResult AddToCart(int id)
        {
            string CustomerId = GetCustomerId();

            // Get menu item by id
            MenuItem item = _context.MenuItems
                            .Where(i => i.MenuItemId == id)
                            .FirstOrDefault();

            ShoppingCart shoppingCart = _context.ShoppingCarts
                                        .Where(s => s.CustomerId == CustomerId)
                                        .FirstOrDefault();

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.CustomerId = CustomerId;
                shoppingCart.Created = DateTime.UtcNow;
                shoppingCart.IsActive = true;
                shoppingCart.Items = new List<MenuItem>();

                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }

            // Add to cart
            if (item != null)
            {
                shoppingCart.Items.Add(item);
            }

            // Save changes
            _context.SaveChanges();

            return Redirect("/ShoppingCart");
        }

        public string GetCustomerId()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("CustomerId")))
            {
                string customerId = "";

                if (User.Identity.IsAuthenticated)
                {
                    customerId = User.Identity.Name;
                }
                else
                {
                    customerId = Guid.NewGuid().ToString();
                }

                HttpContext.Session.SetString("CustomerId", customerId);
            }

            return HttpContext.Session.GetString("CustomerId");
        }


    }
}
