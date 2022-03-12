using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    // Think of the order as the receipt
    // Receipts include general info: date of the transaction, total amount, etc. 
    // as well as a list of items you bought
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryNotes { get; set; }
        public decimal Total { get; set; }

        public string CustomerId { get; set; }

        // list of items you paid for
        // 1 to M relationship
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
