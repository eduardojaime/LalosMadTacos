using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LalosMadTacos.Models
{
    public class MenuItemShoppingCart
    { 
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

    }
}
