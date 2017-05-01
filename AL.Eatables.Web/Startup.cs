using AL.Eatables.Web.Core.Services.InventoryService;
using AL.Eatables.Web.Core.Services.InventoryStatusTicker;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(AL.Eatables.Web.Startup))]

namespace AL.Eatables.Web
{
    public class Startup
    {
        private InventoryStatusTicker _inventoryUpdater;

        public void Configuration(IAppBuilder app)
        {
            _inventoryUpdater = new InventoryStatusTicker(new InventoryService());

            app.Map("/inventory-signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                map.RunSignalR(new HubConfiguration { EnableDetailedErrors = true });
            });
        }
    }
}
