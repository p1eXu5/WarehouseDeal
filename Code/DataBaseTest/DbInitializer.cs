using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataBaseTest
{
    class DbInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed (DataContext context)
        {
            var products = new Product[]
            {
                new Product {Name = "Гитара", Coast = 499.99},
                new Product {Name = "Синтезатор", Coast = 399.99},
                new Product {Name = "Бас-гитара", Coast = 499.99},
            };

            var customers = new Customer[]
            {
                new Customer {Name = "Yamaha"},
                new Customer {Name = "Casio"},
                new Customer {Name = "Jackson"},
            };

            context.Customers.AddRange (customers);
            context.Products.AddRange (products);

            var productCustomerSet = new[]
            {
                new ProductCustomer {Customer = customers[0], Product = products[0], Count = 10},
                new ProductCustomer {Customer = customers[0], Product = products[1], Count = 5},
                new ProductCustomer {Customer = customers[0], Product = products[2], Count = 2},
                new ProductCustomer {Customer = customers[1], Product = products[1], Count = 8},
                new ProductCustomer {Customer = customers[2], Product = products[0], Count = 9},
                new ProductCustomer {Customer = customers[2], Product = products[2], Count = 1}
            };

            context.ProductCustomer.AddRange (productCustomerSet);

            base.Seed (context);
        }
    }
}
