
namespace WarehouseDeal.BaseClasses.Tests.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WarehouseDeal.BaseClasses;

    [TestClass]
    public class ObservableObjectTests
    {
        [TestMethod]
        public void PropertyChangedHandlerRaised()
        {
            var obj = new StubObservableObject();

            bool raised = false;

            obj.PropertyChanged += (s, e) =>
            {
                Assert.IsTrue (e.PropertyName == "ChangedProperty");
                raised = true;
            };

            obj.ChangedProperty = "SomeProperty";

            Assert.IsTrue (raised);
        }

        class StubObservableObject : ObservableObject
        {
            private string _changedProperty;

            public string ChangedProperty
            {
                get => _changedProperty;
                set {
                    _changedProperty = value;
                    NotifyPropertyChanged ();
                }
            }
        }
    }
}
