namespace WarehouseDeal.BaseClasses.Tests.UnitTests
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void ViewModel_IsAbstractClass()
        {
            Type t = typeof(ViewModel);

            Assert.IsTrue (t.IsAbstract);
        }

        [TestMethod]
        public void IsDataErrorInfo()
        {
            Assert.IsTrue (typeof(IDataErrorInfo).IsAssignableFrom (typeof (ViewModel)));
        }

        [TestMethod]
        public void IndexerPropertyValidatesPropertyNameWithInvalidValue()
        {
            var obj = new StubViewModel ();

            Assert.IsNotNull (obj["RequiredProperty"]);
        }

        [TestMethod]
        public void IndexerPropertyValidatesPropertyNameWithValidValue ()
        {
            var obj = new StubViewModel
            {
                RequiredProperty = "Some property"
            };

            Assert.IsNull (obj["RequiredProperty"]);
        }

        class StubViewModel : ViewModel
        {
            [Required]
            public string RequiredProperty { get; set; }

            [Required]
            [StringLength(32, MinimumLength = 4)]
            public string SomeProperty { get; set; }
        }
    }
}
