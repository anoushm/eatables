using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ALServices = AL.Eatables.Web.Core.Services.InventoryService;
using ALModels = AL.Eatables.Web.Models;
using System.Linq;

namespace AL.Eatable.UnitTests.Core.Services.InventoryServices
{
    [TestClass]
    public class InventoryService
    {
        protected ALServices.InventoryService _inventoryService;
        protected ALModels.Eatable _eatable;

        public InventoryService()
        {
            _inventoryService = new ALServices.InventoryService();
            _eatable = CreateEatable();
        }

        [TestClass]
        public class WhenUpdateIsCalled : InventoryService
        {
            [TestMethod]
            public void ShouldBeableToCallWithExistingEatable()
            {
                _inventoryService.Update(1, _eatable);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ShouldBeableToCallWithNonexistingEatable()
            {
                _inventoryService.Update(-1, _eatable);
            }
        }

        [TestClass]
        public class WhenRemoveIsCalled : InventoryService
        {
            [TestMethod]
            public void ShouldBeableToCallRemoveExistingEatable()
            {
                var newEatable = _inventoryService.Insert(new ALModels.Eatable { Name = "new-eatable", Price = 1 });
                _inventoryService.Remove(newEatable.Id);
            }

            [TestMethod]
            [ExpectedException(typeof(System.InvalidOperationException))]
            public void ShouldBeableToCallRemoveNonexistingEatable()
            {
                _inventoryService.Remove(-1);
            }
        }

        [TestClass]
        public class WhenGetIsCalled : InventoryService
        {
            [TestMethod]
            public void ShouldReturnValidEatables()
            {
                var result = _inventoryService.Get();

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
            }

            [TestMethod]
            public void ShouldReturnValidEatablesWhenPassingValidId()
            {
                var result = _inventoryService.Get(2);

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void ShouldReturnValidEatablesWhenPassingInvalidId()
            {
                var result = _inventoryService.Get(-1);

                Assert.IsNull(result);
            }
        }

        [TestClass]
        public class WhenGetAsGroupIsCalled : InventoryService
        {
            [TestMethod]
            public void ShouldReturnValidEatables()
            {
                var result = _inventoryService.GetAsGroup();

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Count > 0);
                Assert.IsTrue(result[0].Count > 0);
                Assert.IsTrue(result[0].Price > 0);
            }
        }

        [TestClass]
        public class WhenInsertIsCalled : InventoryService
        {
            [TestMethod]
            public void ShouldBeableToCallInsertWithNewEatable()
            {
                _inventoryService.Insert(_eatable);
            }
        }

        [TestClass]
        public class WhenCountsAreCalled : InventoryService
        {
            [TestMethod]
            public void ShouldGetAccurateInventoryCount()
            {
                var inventoryList = _inventoryService.Get();
                var countResult = _inventoryService.InventoryCount;

                Assert.IsTrue(inventoryList.Count == countResult);
            }

            [TestMethod]
            public void ShouldGetAccurateSalesCount()
            {
                var preSalesTotal = _inventoryService.SalesTotal;
                _inventoryService.Remove(2);

                var salesTotlaResult = _inventoryService.SalesCount;

                Assert.IsTrue(salesTotlaResult == preSalesTotal + 1);
            }
        }

        [TestClass]
        public class WhenTotalsAreCalled : InventoryService
        {
            [TestMethod]
            public void ShouldGetAccurateInventoryTotalCount()
            {
                var expectedResult = _inventoryService.Get().Sum(e => e.Price);
                var actualResult = _inventoryService.InventoryTotal;

                Assert.IsTrue(expectedResult == actualResult);
            }

            [TestMethod]
            public void ShouldGetAccurateSalesTotalCount()
            {
                var preSalesTotal = _inventoryService.SalesTotal;
                var item3 = _inventoryService.Get(3);
                _inventoryService.Remove(3);
                var expectedResult = preSalesTotal + item3.Price;

                var actualResult = _inventoryService.SalesTotal;

                Assert.IsTrue(expectedResult == actualResult);
            }
        }

        protected ALModels.Eatable CreateEatable()
        {
            return new ALModels.Eatable { Name = "whatever", Price = 1 };
        }
    }
}
