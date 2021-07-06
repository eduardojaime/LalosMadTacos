using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LalosMadTacos.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }

        [Required] // Not null field
        public string Name { get; set; }

        [Required]
        [Range(1.0, 500.0)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        // 1 to many relationship
        public int CategoryId { get; set; }
        // Trick > for easier access of data
        public Category Category { get; set; }

        public string Image { get; set; } // /images/taco.png
    }
}
