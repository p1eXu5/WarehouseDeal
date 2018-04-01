using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarehouseDeal.Data.Tests;
using WarehouseDeal.DesktopClient.ViewModels;

namespace WarehouseDeal.DesktopClient.Tests
{
    [TestClass]
    public class CategoriesModelViewTests :DatabaseScenarioTests
    {
        [TestMethod]
        public void IsCategoryStringReturnsFalseWhenLineIsNotCorrect()
        {
            var viewModel = new CategoriesModelView();

            string[] line = { "12345678", "Some CategoryName", "Да", "1234567" };
            Assert.IsFalse (viewModel.IsCategoryString (line));

            line = new []   { "1234567", "Some CategoryName", "Да", "12345678" };
            Assert.IsFalse (viewModel.IsCategoryString (line));

            line = new[]    { "1234567", "Some CategoryName", "Да", "123456" };
            Assert.IsFalse (viewModel.IsCategoryString (line));

            line = new[]    { "123456", "Some CategoryName", "Да", "1234567" };
            Assert.IsFalse (viewModel.IsCategoryString (line));

            line = new[]    { "1234567", "", "Да", "1234567" };
            Assert.IsFalse (viewModel.IsCategoryString (line));

            line = new[]    { "1234567", null, "Да", "1234567" };
            Assert.IsFalse (viewModel.IsCategoryString (line));

            line = new[] { "", "Some CategoryName", "Да", "1234567" };
            Assert.IsFalse (viewModel.IsCategoryString (line));

            line = new[] { null, "Some CategoryName", "Да", "1234567" };
            Assert.IsFalse (viewModel.IsCategoryString (line));
        }

        [TestMethod]
        public void IsCategoryStringReturnsTrueWhenLineIsCorrect ()
        {
            var viewModel = new CategoriesModelView ();

            string[] line = { "1234567", "Some CategoryName", "Да", "1234567" };
            Assert.IsTrue (viewModel.IsCategoryString (line));

            line = new[] { "1234567", "Some CategoryName", "Да", "" };
            Assert.IsTrue (viewModel.IsCategoryString (line));

            line = new[] { "1234567", "Some CategoryName", "Да", null };
            Assert.IsTrue (viewModel.IsCategoryString (line));
        }
    }
}
