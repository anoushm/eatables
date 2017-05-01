using AL.Eatables.Web.Core.Services.InventoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AL.Eatables.Web.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InventorySummaryController : ApiController
    {
        private IInventoryService _inventoryService;

        public InventorySummaryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }


        public IHttpActionResult Get()
        {
            return Ok(_inventoryService.GetAsGroup());
        }
    }
}
