using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarehouseDeal.Data.Tests
{
    [TestClass]
    public class DatabaseScenarioTests
    {
        [TestInitialize]
        public virtual void CanCreateDatabase ()
        {
            Debug.Print ("\nRunning CanCreateDatabase");

            using (var db = new DataContext()) {

                if(!db.Database.Exists())
                    db.Database.Create();
            }
        }

        [TestCleanup]
        public virtual void ClassCleanup ()
        {
            Debug.Print ("\nRunning ClassCleanup1");

            using (var db = new DataContext ()) {

                if (db.Database.Exists ())
                    db.Database.Delete ();
            }
        }
    }
}
