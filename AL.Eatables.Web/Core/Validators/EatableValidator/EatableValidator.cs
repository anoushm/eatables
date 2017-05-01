using AL.Eatables.Web.Models;
using System;

namespace AL.Eatables.Web.Core.Validators.EatableValidator
{
    public class EatableValidator : IEatableValidator
    {
        public EatableValidator()
        { }

        public bool IsValid(EatableInput request)
        {
            if (request == null || String.IsNullOrEmpty(request.Name) || request.Price < 0)
                return false;

            return true;
        }
    }
}