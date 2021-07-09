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
            // for now just get id == 100
            ShoppingCart shoppingCart = _context.ShoppingCarts
                                        .Where(s => s.AspNetUserId == 100)
                                        .FirstOrDefault();
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.AspNetUserId = 100;
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
            // Get menu item by id
            MenuItem item = _context.MenuItems
                            .Where(i => i.MenuItemId == id)
                            .FirstOrDefault();

            ShoppingCart shoppingCart = _context.ShoppingCarts
                                        .Where(s => s.AspNetUserId == 100)
                                        .FirstOrDefault();
            
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.AspNetUserId = 100;
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


    }
}
