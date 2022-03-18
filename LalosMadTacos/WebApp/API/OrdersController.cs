using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
// Import necessary namespaces
using WebApp.Data;
using System.Linq;

namespace WebApp.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        // configure dependency injection
        public OrdersController(ApplicationDbContext context) { 
            _context = context;
        }

        [Route("Orders90Days")]
        // Show orders in the past 90 days
        public IActionResult GetOrders90Days() { 
            var orders = _context.Orders
                .Where(o => o.OrderDate >= System.DateTime.Now.AddDays(-90))
                .OrderBy(o => o.OrderDate);

            return new JsonResult(orders);            
        }
    }
}
