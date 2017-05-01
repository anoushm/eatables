using Web = AL.Eatables.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Linq;
using AL.Eatables.Web.Core.Services.InventoryService;
using AL.Eatables.Web.Core.Validators.EatableValidator;

namespace AL.Eatables.IntegrationTests.Controllers
{
    [TestClass]
    public class InventorySummaryController
    {
        protected readonly Web.Controllers.InventorySummaryController _controller;

        public InventorySummaryController()
        {
            _controller = new Web.Controllers.InventorySummaryController(
                new InventoryService());
        }
        [TestMethod]
        public void Integration_ShouldGetAsGroupListOfEatables()
        {
            var response = _controller.Get();
            var inventoryList = ((OkNegotiatedContentResult<IList<Web.Models.EatableGroup>>)response).Content.ToList();

            Assert.IsNotNull(inventoryList);
            Assert.IsTrue(inventoryList.Count > 0);
            Assert.IsTrue(inventoryList[0].Count > 0);
            Assert.IsTrue(inventoryList[1].Count > 0);
            Assert.IsTrue(inventoryList[2].Count > 0);
        }
    }
}
