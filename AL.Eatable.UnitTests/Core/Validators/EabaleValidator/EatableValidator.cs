using Web = AL.Eatables.Web.Core.Validators.EatableValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AL.Eatable.UnitTests.Core.Validators.EabaleValidator
{
    [TestClass]
    public class EatableValidator
    {
        private Web.EatableValidator _validator;

        public EatableValidator()
        {
            _validator = new Web.EatableValidator();
        }

        [TestMethod]
        public void ShouldReturnTrueWhenEatableIsValid()
        {
            var result = _validator.IsValid(new AL.Eatables.Web.Models.Eatable { Id = 1, Name = "whatever", Price = 1 });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldReturnTrueWhenEatableIsNull()
        {
            var result = _validator.IsValid(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ShouldReturnTrueWhenEatableWithoutName()
        {
            var result = _validator.IsValid(new AL.Eatables.Web.Models.Eatable { Id = 1, Name = "", Price = 1 });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ShouldReturnTrueWhenEatableWithNegativePrice()
        {
            var result = _validator.IsValid(new AL.Eatables.Web.Models.Eatable { Id = 1, Name = "whatever", Price = -1 });

            Assert.IsFalse(result);
        }
    }
}
