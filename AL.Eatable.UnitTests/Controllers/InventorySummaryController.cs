using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web = AL.Eatables.Web;
using System.Web.Http.Results;
using System.Collections.Generic;
using Moq;
using AL.Eatables.Web.Core.Services.InventoryService;
using AL.Eatables.Web.Core.Validators.EatableValidator;
using System.Linq;

namespace AL.Eatable.UnitTests.Controllers
{
    [TestClass]
    public class InventorySummaryController
    {
        private readonly Web.Controllers.InventorySummaryController _controller;
        private readonly IList<Web.Models.EatableGroup> _mockInventoryGroupList;
        private readonly Mock<IInventoryService> _inventoryServiceMock;

        public InventorySummaryController()
        {
            _inventoryServiceMock = new Mock<IInventoryService>();
            _mockInventoryGroupList = CreatMockInventoryGroupList();
            _inventoryServiceMock.Setup(i => i.GetAsGroup()).Returns(_mockInventoryGroupList);

            _controller = new Web.Controllers.InventorySummaryController(
                _inventoryServiceMock.Object);

        }

        [TestMethod]
        public void ShouldReturnValidGroupedInventoryListWhenGetIsCalled()
        {
            var response = _controller.Get();
            var inventorGroupList = ((OkNegotiatedContentResult<IList<Web.Models.EatableGroup>>)response).Content.ToList();

            Assert.IsNotNull(inventorGroupList);
            Assert.IsTrue(inventorGroupList.Count == _mockInventoryGroupList.Count);
            Assert.IsTrue(inventorGroupList[0].Count == _mockInventoryGroupList[0].Count);
            Assert.IsTrue(inventorGroupList[0].Price == _mockInventoryGroupList[0].Price);
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

        private IList<Web.Models.EatableGroup> CreatMockInventoryGroupList()
        {
            var inventory = CreatMockInventoryList();

            return inventory.GroupBy(e => new { e.Name }).Select(e =>
                    new Web.Models.EatableGroup
                    {
                        Name = e.Key.Name,
                        Count = e.Count(),
                        Price = e.Sum(x => Math.Round(Convert.ToDecimal(x.Price), 2))
                    }).ToList();
        }
    }
}
