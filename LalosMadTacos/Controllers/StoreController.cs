using LalosMadTacos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LalosMadTacos.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        // Using DI to get the configuration object from the app and the AppDbContext
        public StoreController(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(_dbContext.Categories.OrderBy(c => c.Name).ToList());
        }
    }
}
