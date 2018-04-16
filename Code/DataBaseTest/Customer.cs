using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTest
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Customer()
        {
            ProductCustomer = new HashSet<ProductCustomer> ();
        }

        [Required]
        public ICollection<ProductCustomer> ProductCustomer { get; set; }
    }
}
