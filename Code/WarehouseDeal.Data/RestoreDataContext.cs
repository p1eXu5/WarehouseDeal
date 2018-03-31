using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data
{
    class RestoreDataContext : DbContext
    {
        public RestoreDataContext ()
            : base("Default")
        {
        }

        protected override void OnModelCreating (DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category> ()
                .HasOptional (p => p.CategoryParent)
                .WithOptionalPrincipal (c => c.CategoryChild)
                .WillCascadeOnDelete (false);

            modelBuilder.Entity<Category> ()
                .HasMany (p => p.Product)
                .WithRequired (c => c.Category)
                .HasForeignKey (k => k.CategoryId)
                .WillCascadeOnDelete (false);
        }
    }
}
