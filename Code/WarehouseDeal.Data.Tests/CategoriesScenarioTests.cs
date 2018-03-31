using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarehouseDeal.Data.Tests
{
    [TestClass]
    public class CategoriesScenarioTests
    {
        [TestMethod]
        public void AddNewCategoryWithValidValues()
        {
            using (var bc = new BusinessContext()) {

                Category entity = bc.AddNewCategory ("AM18162",
                    "01. Аксессуары (Чехлы и сумки, Манипуляторы, Аудиотехника)", true, null);

                Assert.IsNotNull (entity);
            }
        }
    }
}
