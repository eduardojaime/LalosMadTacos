﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
// Make sure to import needed namespaces
using WebApp.Data;
using System.Linq;
using WebApp.Models;

namespace WebApp.API
{
    // Also make sure to associate this controller to a route
    // uses will have to make a request to /api/menu to access these action methods
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : Controller
    {
        // 1) Configure dependency injection to have access to the db context
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -----------------------------------------------------------------

        // 2) Add routes
        // GET /api/Menu > list of menu items
        // without route parameter, action method is tied to an http verb (get by default)
        public IActionResult GetMenu()
        {
            var menuItems = _context.Products.OrderBy(p => p.Name).ToList();
            return new JsonResult(menuItems);
        }

        // GET /api/Menu/FilteredMenu?name={something} > filtered list of menu items that include the provided word in the name
        // to specify the name of the action, use Route
        [Route("FilteredMenu")] // user friendly URL
        public IActionResult FilteredMenu(string name) // internal name of your method
        {
            // SELECT products WHERE name of product contains name provided
            var menuItems = _context.Products.Where(p => p.Name.Contains(name)).OrderBy(p => p.Name).ToList();
            return new JsonResult(menuItems);
        }

        // CRUD Operations

        // CREATE > POST
        [HttpPost]
        // public IActionResult PostMenuItem([Bind("Id,Name,Price,Description,ImageUrl,CategoryId")] Product product)
        public IActionResult PostMenu(string Name, decimal Price, string Description, string ImageUrl, int CategoryId)
        {
            // Add validations
            if (string.IsNullOrEmpty(Name))
                return new JsonResult("Name cannot be empty");
            if (Price <= 0)
                return new JsonResult("Price value must be greater than 0");
            if (string.IsNullOrEmpty(Description))
                return new JsonResult("Description cannot be empty");
            if (CategoryId <= 0)
                return new JsonResult("Please enter a valid category id");

            // Create a product object and add it to db
            Product product = new Product();
            product.Name = Name;
            product.Price = Price;
            product.Description = Description;
            product.ImageUrl = ImageUrl;
            product.CategoryId = CategoryId;

            _context.Products.Add(product);
            _context.SaveChanges();

            // return newly added product to DB
            return new JsonResult(product);
        }

        // UPDATE > PUT
        // PUT /api/Menu/1
        [HttpPut("{id}")]
        public IActionResult PutMenuItem()
        {
            return new JsonResult("Not implemented");
        }

        // DELETE > DELETE
        // DELETE /api/Menu/2
        [HttpDelete("{id}")]
        public IActionResult DeleteMenuItem()
        {
            return new JsonResult("Not implemented");
        }

        // Orders by day report
    }
}
