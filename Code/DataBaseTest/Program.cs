using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTest
{
    class Program
    {
        static void Main (string[] args)
        {
            using (var db = new DataContext()) {


                // Получени данных
                var products = db.Products.Include("ProductCustomer").ToArray();     // Local не вызывает заполнение DbSet, заполнение вызывает вызов перечислителя в foreach
                                                                                    // ToList/To... обязателен, т.к. в теле цикла будет обращение за чтением данных из бд
                db.Customers.Load();


                foreach (var product in products) {

                    Console.WriteLine ($"{product.Name} ({product.ProductCustomer.Count}):");
                    

                    foreach (var customer in product.ProductCustomer) {

                        Console.WriteLine ($"\t{customer.Customer.Name} {customer.Count}");
                    }
                }

                // Проверка на наличие сущности
                var p = products[0];
                ((IEnumerable<Product>)db.Products).Contains(p);
                if (db.Products.Find (products[0].Id) == null) {

                    var productFakeNew = products[0];
                    db.Products.Add (productFakeNew);
                    db.SaveChanges();   // Добавляет запись с новым id и вызывает исключение
                }
                else {
                    Console.WriteLine ("Элемент уже есть");
                }

                // HashSet

                ISet<Product> productSet1 = new HashSet<Product>(db.Products);
                IEnumerable<Product> productSet2 = new List<Product>(products.Where (pr => pr.Id < 3));


                Action lambda = () =>
                                    {
                                        Console.WriteLine ($"\nSet 1:");

                                        foreach (var product in productSet1)
                                            Console.WriteLine ($"{product.Id}: {product.Name}");
                                    };
                lambda();

                Console.WriteLine ($"\nSet 2:");

                foreach (var product in productSet2) 
                    Console.WriteLine ($"{product.Id}: {product.Name}");
                
                productSet1.ExceptWith (productSet2);
                lambda();
            }
        }
    }

    public class MyComparer : IEqualityComparer<Product>
    {
        public bool Equals (Product x, Product y)
        {
            return string.Equals (x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode (Product obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
