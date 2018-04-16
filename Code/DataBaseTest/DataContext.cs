using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataBaseTest
{
    public class DataContext : DbContext
    {
        public DataContext() : base ("Default")
        {
            Database.SetInitializer (new DbInitializer());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductCustomer> ProductCustomer { get; set; }

        protected override void OnModelCreating (DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCustomer> ().HasKey (t => new {t.ProductId, t.CustomerId});
            modelBuilder.Entity<Product>().HasMany (e => e.ProductCustomer).WithRequired (pc => pc.Product);
            modelBuilder.Entity<Customer>().HasMany (c => c.ProductCustomer).WithRequired (pc => pc.Customer);

            base.OnModelCreating (modelBuilder);
        }
    }
}
