// Use annotiations to handle: validations, formats, display labels (used in views)
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Display price in currency format: $ 123.45
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // 1 to M relationship
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // Automatically links categoryId to a Category object
        // virtual navigation (c# only) so we don't have to use joins to connect related records
        public Category Category { get; set; }
    }
}
