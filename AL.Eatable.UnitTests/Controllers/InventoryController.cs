using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web = AL.Eatables.Web;
using Moq;
using AL.Eatables.Web.Core.Services.InventoryService;
using AL.Eatables.Web.Core.Validators.EatableValidator;
using System.Collections.Generic;
using AL.Eatables.Web.Models;
using System.Linq;
using System.Web.Http.Results;

namespace AL.Eatable.UnitTests.Controllers
{
    [TestClass]
    public class InventoryController
    {
        private readonly Web.Controllers.InventoryController _controller;
        private readonly Mock<IInventoryService> _inventoryServiceMock;
        private readonly Mock<IEatableValidator> _eatableValidatorMock;
        private readonly IList<Web.Models.Eatable> _mockInventoryList;
        private readonly Web.Models.Eatable _newEatable;

        public InventoryController()
        {
            Web.App_Start.MapperConfig.Configure();
            _newEatable = new Web.Models.Eatable { Name = "new-eatable", Price = 0.1M };
            _inventoryServiceMock = new Mock<IInventoryService>();
            _eatableValidatorMock = new Mock<IEatableValidator>();
            _mockInventoryList = CreatMockInventoryList();
 
            _eatableValidatorMock.Setup(e => e.IsValid(It.IsAny<Web.Models.EatableInput>())).Returns(true);
            _inventoryServiceMock.Setup(i => i.Get()).Returns(_mockInventoryList);
            _inventoryServiceMock.Setup(i => i.Get(It.IsAny<int>())).Returns(_mockInventoryList.FirstOrDefault());
            _inventoryServiceMock.Setup(i => i.Insert(It.IsAny<Web.Models.Eatable>())).Returns(_newEatable);
            _inventoryServiceMock.Setup(i => i.Remove(It.IsAny<int>()));
            _inventoryServiceMock.Setup(i => i.Update(It.IsAny<int>(), It.IsAny<Web.Models.Eatable>()));

            _controller = new Web.Controllers.InventoryController(
                _inventoryServiceMock.Object,
                _eatableValidatorMock.Object);
        }

        [TestMethod]
        public void ShouldReturnValidInventoryListWhenGetIsCalled()
        {
            var response = _controller.Get();
            var inventoryList = ((OkNegotiatedContentResult<IList<Web.Models.Eatable>>)response).Content.ToList();

            Assert.IsNotNull(inventoryList);
            Assert.IsTrue(inventoryList.Count == _mockInventoryList.Count);
        }

        [TestMethod]
        public void ShouldReturnValidInventoryItemWhenGetIsCalledWithId()
        {
            var response = _controller.Get(1);
            var inventoryItem = ((OkNegotiatedContentResult<Web.Models.Eatable>)response).Content;

            Assert.IsNotNull(inventoryItem);
            Assert.IsTrue(inventoryItem.Name == _mockInventoryList.First().Name);
        }

        [TestMethod]
        public void ShouldReturnValidInventoryListWhenPostValidEatableItem()
        {
            var response = _controller.Post(new Web.Models.EatableInput());
            var newEatabelItem = ((OkNegotiatedContentResult<Web.Models.Eatable>)response).Content;

            Assert.IsNotNull(newEatabelItem);
            Assert.AreEqual(newEatabelItem, _newEatable);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Web.Http.HttpResponseException))]
        public void ShouldReturnThrowExceptionWhenPostWithIsValidReturnsFalse()
        {
            _eatableValidatorMock.Setup(e => e.IsValid(It.IsAny<Web.Models.Eatable>())).Returns(false);

            _controller.Post(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldThrowExceptionWhenInsertIsFaling()
        {
            _inventoryServiceMock.Setup(i => i.Insert(It.IsAny<Web.Models.Eatable>())).Throws(new Exception());

            _controller.Post(null);
        }

        [TestMethod]
        public void ShouldReturnValidInventoryListWhenPutValidEatableItem()
        {
            var etableName = "whatever";
            var etableId = 2;

            var response = _controller.Put(etableId, new Web.Models.EatableInput { Name = etableName, Price = 1.0M });
            var inventoryList = ((OkNegotiatedContentResult<IList<Web.Models.Eatable>>)response).Content.ToList();

            Assert.IsNotNull(inventoryList);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Web.Http.HttpResponseException))]
        public void ShouldReturnThrowExceptionWhenPutWithIsValidReturnsFalse()
        {
            _eatableValidatorMock.Setup(e => e.IsValid(It.IsAny<Web.Models.Eatable>())).Returns(false);

            _controller.Put(1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldThrowExceptionWhenUpdateIsFaling()
        {
            _inventoryServiceMock.Setup(i => i.Update(It.IsAny<int>(), It.IsAny<Web.Models.Eatable>())).Throws(new Exception());

            _controller.Put(0, null);
        }

        [TestMethod]
        public void ShouldReturnValidInventoryListWhenDeleteItem()
        {
            var etableId = 2;

            var response = _controller.Delete(etableId);
            var inventoryList = ((OkNegotiatedContentResult<IList<Web.Models.Eatable>>)response).Content.ToList();

            Assert.IsNotNull(inventoryList);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldReturnThrowExceptionWhenDeleteFails()
        {
            _inventoryServiceMock.Setup(i => i.Remove(It.IsAny<int>())).Throws(new Exception());

            _controller.Delete(1);
        }

        private IList<Web.Models.Eatable> CreatMockInventoryList()
        {
            return new List<Web.Models.Eatable> {
                new Web.Models.Eatable { Id = 1, Name = "Taco", Price = 3.0M },
                new Web.Models.Eatable { Id = 2, Name = "Taco", Price = 3.0M },
                new Web.Models.Eatable { Id = 3, Name = "Kabob", Price = 6.0M },
                new Web.Models.Eatable { Id = 4, Name = "Pizza", Price = 4.0M },
                new Web.Models.Eatable { Id = 5, Name = "Sushi", Price = 11.0M }
            };
        }
    }
}
