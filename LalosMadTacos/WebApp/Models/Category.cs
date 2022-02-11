using System.Collections.Generic;

namespace WebApp.Models
{
    public class Category // this will become the Category table in SQL Server
    {
        // think about what data we want for a Category
        public int Id { get; set; }
        public string Name { get; set; }

        // A category will contain a list of products
        // 1 to M relationship
        // Not a database field
        // This field will exist only in C#
        // This does automatic filtering of products by category id
        public List<Product> Products { get; set; }
    }
}
