using System.Collections.Generic;
using System.Linq;
using AL.Eatables.Web.Models;
using Microsoft.AspNet.SignalR;
using AL.Eatables.Web.Hubs;
using System;

namespace AL.Eatables.Web.Core.Services.InventoryService
{
    public class InventoryService : IInventoryService
    {
        private static readonly List<Eatable> _inventoryList = InitialzeInventoryList();
        private static readonly List<Eatable> _salesList = new List<Eatable>();
        private static readonly object _lock = new object();
        private IHubContext _hub;

        public int InventoryCount { get { return _inventoryList.Count; } }
        public int SalesCount { get { return _salesList.Count; } }
        public decimal InventoryTotal { get { return _inventoryList.Sum(i => i.Price); } }
        public decimal SalesTotal { get { return _salesList.Sum(i => i.Price); } }

        public InventoryService()
        {
            _hub = GlobalHost.ConnectionManager.GetHubContext<InventoryHub>();
        }

        public IList<Eatable> Get()
        {
            return _inventoryList;
        }

        public Eatable Get(int id)
        {
            return _inventoryList.FirstOrDefault(i => i.Id == id);
        }


        public IList<EatableGroup> GetAsGroup()
        {
            return _inventoryList.GroupBy(e => new { e.Name }).Select(e => new EatableGroup
            {
                Name = e.Key.Name,
                Count = e.Count(),
                Price = e.Sum(x => Math.Round(Convert.ToDecimal(x.Price), 2))
            }).ToList();
        }

        public Eatable Insert(Eatable eatable)
        {
            lock (_lock)
            {
                eatable.Id = GenerateUniqId();

                _inventoryList.Add(eatable);
                BoradcastInventory();
            }

            return eatable;
        }

        public void Remove(int id)
        {
            lock (_lock)
            {
                var item = _inventoryList.First(i => i.Id == id);

                _salesList.Add(item);
                _inventoryList.Remove(item);
                BoradcastInventory();
            }
        }

        public void Update(int id, Eatable eatable)
        {
            var item = _inventoryList.First(i => i.Id == id);

            _inventoryList.Remove(item);
            _inventoryList.Add(eatable);
        }

        private static List<Eatable> InitialzeInventoryList()
        {
            return new List<Eatable> {
                new Eatable { Id = 1, Name = "Cookies", Price = 2.0M },
                new Eatable { Id = 2, Name = "Swedish-Fish Candy", Price = 1.0M },
                new Eatable { Id = 3, Name = "Taco", Price = 3.0M },
                new Eatable { Id = 4, Name = "Kabob", Price = 6.0M },
                new Eatable { Id = 5, Name = "Pizza", Price = 4.0M },
                new Eatable { Id = 6, Name = "Sushi", Price = 11.0M }
            };
        }

        private void BoradcastInventory()
        {
            _hub.Clients.All.broadcastInventory(_inventoryList);
        }

        private int GenerateUniqId()
        {
            var largestId = _inventoryList.OrderByDescending(i => i.Id).FirstOrDefault()?.Id ?? 0;

            return ++largestId;
        }
    }
}