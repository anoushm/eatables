using AL.Eatables.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AL.Eatables.Web.Core.Services.InventoryService
{
    public interface IInventoryService
    {
        IList<Eatable> Get();
        Eatable Get(int id);
        IList<EatableGroup> GetAsGroup();
        Eatable Insert(Eatable eatable);
        void Remove(int id);
        void Update(int id, Eatable eatable);
        int InventoryCount { get; }
        int SalesCount { get; }
        decimal SalesTotal { get; }
        decimal InventoryTotal { get; }
    }
}
