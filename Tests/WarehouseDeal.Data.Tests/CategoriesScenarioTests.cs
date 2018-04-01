using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarehouseDeal.Data.Tests
{
    [TestClass]
    public class CategoriesScenarioTests : DatabaseScenarioTests
    {
        [TestMethod]
        public void AddNewCategoryWithValidValues()
        {
            Debug.Print ("Running AddNewCategoryWithValidValues");

            using (var bc = new BusinessContext()) {

                Category entity = bc.AddNewCategory ("AM18162",
                    "01. Аксессуары (Чехлы и сумки, Манипуляторы, Аудиотехника)");

                Assert.IsNotNull (entity);
            }
        }

        [TestMethod]
        public void AddNewCategoryWithValidValues2 ()
        {
            Debug.Print ("Running AddNewCategoryWithValidValues");

            using (var bc = new BusinessContext ()) {

                Category entity = bc.AddNewCategory ("AM18163",
                    "01. Аксессуары (Чехлы и сумки, Манипуляторы, Аудиотехника)");

                Assert.IsNotNull (entity);
            }
        }

        [TestMethod]
        public void AddParentCategories()
        {
            // TODO:
            Category category1 = new Category
                                    {
                                        Id = "1111111",
                                        Name = "Category 1"
                                    };

            Category category2 = new Category
                                    {
                                        Id = "2222222",
                                        Name = "Category 2"
                                    };

            Category category3 = new Category
                                    {
                                        Id = "3333333",
                                        Name = "Category 3"
                                    };

            using (var bc = new BusinessContext()) {

                bc.AddNewCategory(category1);
                bc.AddNewCategory(category2);
                bc.AddNewCategory(category3);

                bc.AddParentCategory (category1, category2);
                bc.AddParentCategory (category2, category3);

                Category rootCategory = bc.GetRootCategory();

                Assert.IsTrue (rootCategory == category3);

                ICollection<Category> childCategories = (ICollection<Category>)bc.GetChildCategiries(rootCategory);

                Assert.IsTrue(childCategories.Contains (category2));

                childCategories = (ICollection<Category>)bc.GetChildCategiries (category2);

                Assert.IsTrue (childCategories.Contains (category1));
            }
        }
    }
}
