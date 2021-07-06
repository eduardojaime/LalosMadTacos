using LalosMadTacos.Data;
using LalosMadTacos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Diagnostics;

namespace LalosMadTacos.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var connStrg = _configuration.GetConnectionString("DefaultConnection");
            return View();
        }

        public IActionResult Privacy()
        {
            // Simulating going to the DB and getting data
            List<string> list = new List<string>();
            list.Add("Some privacy policy");
            list.Add("Another policy");
            list.Add("Even more! privacy policy");
            return View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
