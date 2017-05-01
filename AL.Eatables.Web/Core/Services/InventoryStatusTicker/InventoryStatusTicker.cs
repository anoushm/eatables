using AL.Eatables.Web.Core.Services.InventoryService;
using AL.Eatables.Web.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading;
using System.Web.Hosting;

namespace AL.Eatables.Web.Core.Services.InventoryStatusTicker
{
    public class InventoryStatusTicker : IRegisteredObject
    {
        private Timer _taskTimer;
        private IHubContext _hub;

        private IInventoryService _inventoryService;

        public InventoryStatusTicker(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            HostingEnvironment.RegisterObject(this);

            _hub = GlobalHost.ConnectionManager.GetHubContext<InventoryHub>();

            _taskTimer = new Timer(OnTimerElapsed, null,
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.5));
        }

        private void OnTimerElapsed(object sender)
        {
            _hub.Clients.All.broadcastStatus(DateTime.UtcNow.ToString(),
                _inventoryService.InventoryCount,
                _inventoryService.SalesCount,
                _inventoryService.InventoryTotal,
                _inventoryService.SalesTotal);
        }

        public void Stop(bool immediate)
        {
            _taskTimer.Dispose();

            HostingEnvironment.UnregisterObject(this);
        }
    }
}