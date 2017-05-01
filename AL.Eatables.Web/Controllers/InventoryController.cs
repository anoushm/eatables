using AL.Eatables.Web.Core.Services.InventoryService;
using AL.Eatables.Web.Core.Validators.EatableValidator;
using AL.Eatables.Web.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AL.Eatables.Web.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InventoryController : ApiController
    {
        private IEatableValidator _eatableValidator;
        private IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService,
                                   IEatableValidator eatableValidator)
        {
            _inventoryService = inventoryService;
            _eatableValidator = eatableValidator;
        }

        public IHttpActionResult Get()
        {
            return Ok(_inventoryService.Get());
        }

        public IHttpActionResult Get(int id)
        {
            return Ok(_inventoryService.Get(id));
        }

        public IHttpActionResult Post([FromBody]EatableInput input)
        {
            if (!_eatableValidator.IsValid(input))
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            var newEatableItem = _inventoryService.Insert(Mapper.Map<Eatable>(input));

            return Ok(newEatableItem);
        }

        public IHttpActionResult Put(int id, [FromBody]EatableInput input   )
        {
            if (!_eatableValidator.IsValid(input))
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            _inventoryService.Update(id, Mapper.Map<Eatable>(input));

            return Ok(_inventoryService.Get());
        }

        public IHttpActionResult Delete(int id)
        {
            _inventoryService.Remove(id);

            return Ok(_inventoryService.Get());
        }
    }

}
