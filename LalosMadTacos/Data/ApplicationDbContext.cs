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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
