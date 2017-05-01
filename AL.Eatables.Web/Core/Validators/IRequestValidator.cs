using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AL.Eatables.Web.Core.Validators
{
    public interface IRequestValidator<T>
    {
        bool IsValid(T request);
    }
}
