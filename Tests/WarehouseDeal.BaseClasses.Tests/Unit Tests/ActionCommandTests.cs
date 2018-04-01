using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarehouseDeal.BaseClasses.Tests.Unit_Tests
{
    [TestClass]
    public class ActionCommandTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfActionParameterIsNull()
        {
            var command = new ActionCommand (null);
        }

        [TestMethod]
        // Выполнение вызывает действие
        public void ExecuteWithoutParameterInvokesAction ()
        {
            bool flag = false;
            var command = new ActionCommand (parametr =>
            {
                flag = true;
                Assert.IsNull (parametr);
            });

            command.Execute ();

            Assert.IsTrue (flag);
        }

        [TestMethod]
        // Выполнение вызывает действие
        public void ExecuteWithParameterInvokesAction()
        {
            bool flag = false;
            var command = new ActionCommand (parametr =>
            {
                flag = true;
                Assert.IsNotNull (parametr);
                Debug.WriteLine ("\n=================================>   " + parametr.GetType().FullName + "\n");
            });

            command.Execute(new object());

            Assert.IsTrue (flag);
        }

        [TestMethod]
        public void CanExecuteIsTrueByDefault()
        {
            ActionCommand command = new ActionCommand((parametr) => { });
            Assert.IsTrue (command.CanExecute (null));
        }

        [TestMethod]
        public void CanExecuteReturnesTrueIfPredicateReturnsTrue()
        {
            ActionCommand command = new ActionCommand ( p => { }, obj => (int)obj == 1 );
            Assert.IsTrue (command.CanExecute (1));
        }

        [TestMethod]
        public void CanExecuteReturnesFalseIfPredicateReturnsFalse ()
        {
            ActionCommand command = new ActionCommand (p => { }, obj => (int)obj == 1);
            Assert.IsFalse (command.CanExecute (0));
        }
    }
}
