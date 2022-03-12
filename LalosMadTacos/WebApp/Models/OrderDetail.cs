namespace WebApp.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        // Which product is this detail for?
        public int ProductId { get; set; }
        // Link to product 1 to M
        public Product Product { get; set; }

        // Which order does this detail belong to?
        public int OrderId { get; set; }
        // Link to order 1 to M
        public Order Order { get; set; }

        // detail fields
        public int Quantity { get; set; }
        // because price can change in the DB for the product
        // but this shouldn't change the value of the price in past orders
        public decimal Price { get; set; } 
        

    }
}
