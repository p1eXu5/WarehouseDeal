using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data
{
    public class TestTable
    {
        public int Field1 { get; set; }
        public string Field2 { get; set; }
    }

    public class TestContext : DbContext
    {
        public TestContext() : base ("Default1") { }

        public DbSet<TestTable> TestTables;
    }
}
