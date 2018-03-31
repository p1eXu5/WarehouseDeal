using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarehouseDeal.ServiceClasses.Tests
{
    [TestClass]
    public class WarehouseDealServiceClassTestsInitializer
    {
        protected string fileName = "test.csv";

        [TestCleanup]
        public virtual void TestInitialize()
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}
