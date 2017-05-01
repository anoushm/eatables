using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using WebServices = AL.Eatables.Web.Core.Services;

namespace AL.Eatable.UnitTests.Core.Services.InventoryStatusTicker
{
    [TestClass]
    public class InventoryStatusTicker
    {
        private WebServices.InventoryStatusTicker.InventoryStatusTicker _ticker;
        private Mock<WebServices.InventoryService.IInventoryService> _mockInventoryService;

        public InventoryStatusTicker()
        {
            _mockInventoryService = new Mock<Eatables.Web.Core.Services.InventoryService.IInventoryService>();
            _mockInventoryService.Setup(i => i.InventoryCount).Verifiable();
            _mockInventoryService.Setup(i => i.SalesCount).Verifiable();
            _mockInventoryService.Setup(i => i.InventoryTotal).Verifiable();
            _mockInventoryService.Setup(i => i.SalesTotal).Verifiable();

            _ticker = new WebServices.InventoryStatusTicker.InventoryStatusTicker(_mockInventoryService.Object);
            Thread.Sleep(5000);
        }

        [TestMethod]
        public void ShouldCallInventoryCountWhenTickerHasStarted()
        {
             _mockInventoryService.Verify(i => i.InventoryCount);
        }

        [TestMethod]
        public void ShouldCallInventoryTotalWhenTickerHasStarted()
        {
            _mockInventoryService.Verify(i => i.InventoryTotal);
        }

        [TestMethod]
        public void ShouldCallSalesCountWhenTickerHasStarted()
        {
            _mockInventoryService.Verify(i => i.SalesCount);
        }

        [TestMethod]
        public void ShouldCallSalesTotalWhenTickerHasStarted()
        {
            _mockInventoryService.Verify(i => i.SalesTotal);
        }
    }
}
