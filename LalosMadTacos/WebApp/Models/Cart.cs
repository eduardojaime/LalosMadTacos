using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    // Contracts
    // Cart = Line Item in the shoppingcart
    public class Cart
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Linking to Product via Product.Id
        public int ProductId { get; set; }

        public Product Product { get; set; }

        // how to identify the customer
        // by default userId in the ASP.NET out-of-the-box identity schema is GUID
        public string CustomerId { get; set; }

    }
}
