using App_Start = AL.Eatables.Web.App_Start;
using Models = AL.Eatables.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace AL.Eatable.UnitTests.Core.Admin
{
    [TestClass]
    public class MapperConfig
    {
        public MapperConfig()
        {
            App_Start.MapperConfig.Configure();
        }

        [TestMethod]
        public void ConfigurationShouldBeValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void ShouldMapEatableInputToEatable()
        {
            var input = new Models.EatableInput
            {
                Name = "whatever",
                Price = 1.0M
            };

            var output = Mapper.Map<Models.Eatable>(input);

            Assert.AreEqual(input.Price, output.Price);
            Assert.AreEqual(input.Name, output.Name);
        }

        [TestMethod]
        public void ShouldMapNullToEatable()
        {
            var output = Mapper.Map<Models.Eatable>(null);

            Assert.IsNull(output);
        }
    }
}
