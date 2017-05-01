using AL.Eatables.Web.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AL.Eatables.Web.Core.Validators
{
    public abstract class RequestValidator<T> : IRequestValidator<T> where T : new()
    {
        public abstract bool IsValid(T  request);
    }
}