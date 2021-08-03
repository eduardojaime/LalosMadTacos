using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LalosMadTacos.Models;

namespace LalosMadTacos.Data
{
    // IdentityDbContext so we can use ASP.NET out of the box identities for authentication/authorization
    // IdentityDbContext implements DbContext anyways
    public class ApplicationDbContext : IdentityDbContext
    {
        // Define tables (dbsets)
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        // M to M between items and carts
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MenuItemShoppingCart>()
                .HasKey(x => new { x.ShoppingCartId, x.MenuItemId });

            builder.Entity<MenuItemShoppingCart>()
                .HasOne<MenuItem>(ms => ms.MenuItem)
                .WithMany(m => m.MenuItemShoppingCarts)
                .HasForeignKey(ms => ms.MenuItemId);

            builder.Entity<MenuItemShoppingCart>()
                .HasOne<ShoppingCart>(ms => ms.ShoppingCart)
                .WithMany(s => s.MenuItemShoppingCarts)
                .HasForeignKey(ms => ms.ShoppingCartId);
        }
    }
}
