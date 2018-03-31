using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarehouseDeal.Data.Tests
{
    [TestClass]
    public class DatabaseScenarioTests
    {
        [TestMethod]
        public void CanCreateDatabase ()
        {
            using (var db = new DataContext()) {

                if(!db.Database.Exists())
                    db.Database.Create();
            }
        }

        //[ClassCleanup]
        //public static void ClassCleanup()
        //{
        //    using (var db = new DataContext ()) {

        //        if (db.Database.Exists())
        //            db.Database.Delete();
        //    }
        //}
    }
}
