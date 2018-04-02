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
    public class BusinessContextScenarioTests : DatabaseScenarioTests
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
        public void GetAllCategoriesMethodTest()
        {
            Category category1 = new Category {Id = "1111111", Name = "Category 1"};
            Category category2 = new Category {Id = "2222222", Name = "Category 2"};
            Category category3 = new Category {Id = "3333333", Name = "Category 3"};

            using (var bc = new BusinessContext()) {

                bc.AddNewCategory (category1);
                bc.AddNewCategory (category2);
                bc.AddNewCategory (category3);

                ICollection<Category> categories = (ICollection<Category>) bc.GetAllCategories();

                Assert.IsTrue (categories.Contains (category1));
                Assert.IsTrue (categories.Contains (category2));
                Assert.IsTrue (categories.Contains (category3));
            }
        }

        [TestMethod]
        public void AddParentCategoriesMethodTest ()
        {
            // TODO:

            Category category1 = new Category { Id = "1111111", Name = "Category 1" };
            Category category2 = new Category { Id = "2222222", Name = "Category 2" };
            Category category3 = new Category { Id = "3333333", Name = "Category 3" };

            using (var bc = new BusinessContext()) {

                bc.AddNewCategory(category1);
                bc.AddNewCategory(category2);
                bc.AddNewCategory(category3);

                bc.AddParentCategory (category1, category2);
                
                Assert.IsTrue (category1.CategoryParent == category2);
                Assert.IsTrue (category2.CategoryChild == category1);


                //Category rootCategory = bc.GetRootCategory();

                //Assert.IsTrue (rootCategory == category3);

                //ICollection<Category> childCategories = (ICollection<Category>)bc.GetChildrenCategories(rootCategory);

                //Assert.IsTrue(childCategories.Contains (category2));

                //childCategories = (ICollection<Category>)bc.GetChildrenCategories (category2);

                //Assert.IsTrue (childCategories.Contains (category1));
            }
        }

        [TestMethod]
        public void IsCategoryStringReturnsFalseWhenLineIsNotCorrect ()
        {
                string[] line = {"12345678", "Some CategoryName", "Да", "1234567"};
                Assert.IsFalse (BusinessContext.Check.IsCategoryString (line));

                line = new[] {"1234567", "Some CategoryName", "Да", "12345678"};
                Assert.IsFalse (BusinessContext.Check.IsCategoryString (line));

                line = new[] {"1234567", "Some CategoryName", "Да", "123456"};
                Assert.IsFalse (BusinessContext.Check.IsCategoryString (line));

                line = new[] {"123456", "Some CategoryName", "Да", "1234567"};
                Assert.IsFalse (BusinessContext.Check.IsCategoryString (line));

                line = new[] {"1234567", "", "Да", "1234567"};
                Assert.IsFalse (BusinessContext.Check.IsCategoryString (line));

                line = new[] {"1234567", null, "Да", "1234567"};
                Assert.IsFalse (BusinessContext.Check.IsCategoryString (line));

                line = new[] {"", "Some CategoryName", "Да", "1234567"};
                Assert.IsFalse (BusinessContext.Check.IsCategoryString (line));

                line = new[] {null, "Some CategoryName", "Да", "1234567"};
                Assert.IsFalse (BusinessContext.Check.IsCategoryString (line));
        }

        [TestMethod]
        public void IsCategoryStringReturnsTrueWhenLineIsCorrect ()
        {
            string[] line = { "1234567", "Some CategoryName", "Да", "1234567" };
            Assert.IsTrue (BusinessContext.Check.IsCategoryString (line));

            line = new[] { "1234567", "Some CategoryName", "Да", "" };
            Assert.IsTrue (BusinessContext.Check.IsCategoryString (line));

            line = new[] { "1234567", "Some CategoryName", "Да", null };
            Assert.IsTrue (BusinessContext.Check.IsCategoryString (line));
        }

        [TestMethod]
        public void CanLoadCategoriesFromFile ()
        {
            using (var bc = new BusinessContext()) {

                int count = bc.LoadCategoriesFromFile (
                    "D:\\programming projects\\C# Projects\\WarehouseDeal\\Code\\WarehouseDeal.Data\\Assets\\Categories.csv");

                Assert.IsTrue (count == 1311);
            }
        }

        
    }
}
