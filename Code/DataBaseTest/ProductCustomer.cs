using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTest
{
    public class ProductCustomer
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Count { get; set; }

        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
