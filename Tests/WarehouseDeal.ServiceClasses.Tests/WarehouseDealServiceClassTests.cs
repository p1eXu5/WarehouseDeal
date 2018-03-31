using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static WarehouseDeal.ServiceClasses.WatehouseDealServiceClass;

namespace WarehouseDeal.ServiceClasses.Tests
{
    [TestClass]
    public class WarehouseDealServiceClassTests : WarehouseDealServiceClassTestsInitializer
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnumeratorReturnsExceptionWithInvalidFileNameParam()
        {
            //string fileName = "Some file name";

            foreach (var item in GetStringsArrayEnumeratorFromCsvFile(fileName)) ;
        }

        [TestMethod]
        public void EnumeratorReturnsValidStringsFromExistFile()
        {
            string string1 = "param11;param12;param13";

            using (FileStream fs = File.Create(fileName))
            using (TextWriter tw = new StreamWriter(fs)) {

                tw.WriteLine(string1);
                tw.WriteLine(string1);

                fileName = fs.Name;
            }

            foreach (var str in GetStringsArrayEnumeratorFromCsvFile(fileName)) {
                
                Assert.IsTrue(String.Join(";",str).Equals(string1));
            }
        }

        [TestMethod]
        public void EnumeratorReturnsStringOnUnknownCsvFile()
        {
            string str = "Some string";

            using (FileStream fs = File.Create (fileName))
            using (TextWriter tw = new StreamWriter(fs)) {
                
                tw.Write(str);

                fileName = fs.Name;
            }

            foreach (string[] item in GetStringsArrayEnumeratorFromCsvFile(fileName)) {
                
                Assert.IsTrue(String.Equals(str, item[0]));
            }
        }
    }
}
