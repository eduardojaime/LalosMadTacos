using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            // Admins should be able to se everything
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Orders.OrderByDescending(o => o.OrderDate).ToListAsync());
            }
            // Customers must only see their own orders
            else
            {
                return View(await _context.Orders
                                            .Where(o => o.CustomerId == User.Identity.Name) //CustomerId == email address used as username
                                            .OrderByDescending(o => o.OrderDate)
                                            .ToListAsync());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                                .Include(o => o.OrderDetails) // indicate that we need to include order details and products
                                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // REMOVE CREATE EDIT AND DELETE

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
