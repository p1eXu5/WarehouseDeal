using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarehouseDeal.Data.Tests;
using WarehouseDeal.DesktopClient.ViewModels;
using WarehouseDeal.Data;

namespace WarehouseDeal.DesktopClient.Tests
{
    [TestClass]
    public class CategoriesModelViewTests : DatabaseScenarioTests
    {
        //[TestMethod]
        //public void CanSetCategoriesListsFromDatabase ()
        //{
        //    var viewModel = new StubViewModel();
        //    int count = viewModel.Context.LoadCategoriesFromFile (testFile);

        //    viewModel.SetCategoriesLists ();

        //    int countFromModel = viewModel.Categories.Count<SelectedCategory> ();

        //    Assert.IsTrue (count == countFromModel);
        //}

        //private class StubViewModel : MainModelView
        //{
        //    public CategoryContext Context => _context;
        //}
    }
}
