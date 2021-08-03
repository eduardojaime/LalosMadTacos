using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LalosMadTacos.Models
{
    public class ShoppingCart
    {   
        public int ShoppingCartId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Purchased { get; set; }
        public bool IsActive { get; set; }
        public IList<MenuItemShoppingCart> MenuItemShoppingCarts { get; set; }
        // Use this field to identify the customer > anonymous and authenticated users
        public string CustomerId { get; set; }
    }
}