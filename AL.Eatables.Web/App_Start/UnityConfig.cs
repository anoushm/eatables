using AL.Eatables.Web.Core.Services.InventoryService;
using AL.Eatables.Web.Core.Validators.EatableValidator;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace AL.Eatables.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IInventoryService, InventoryService>();
            container.RegisterType<IEatableValidator, EatableValidator>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}