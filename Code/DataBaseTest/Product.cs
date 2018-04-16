using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTest
{
    public class Product
    {
        public Product()
        {
            ProductCustomer = new HashSet<ProductCustomer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Coast { get; set; }

        [Required]
        public ICollection<ProductCustomer> ProductCustomer { get; set; }
    }
}
