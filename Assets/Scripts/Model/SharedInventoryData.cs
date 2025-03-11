using System.Collections.Generic;
using Game.Interface.UI.Inventory;
using Game.UI.Model.Inventory;

namespace Game.UI.Inventory
{
    public class SharedInventoryData : ISharedInventoryData
    {
        private readonly List<Item> items = new();

        public IReadOnlyList<Item> OwnedItems => items;

        public SharedInventoryData(Item[] items)
        {
            this.items.AddRange(items);
        }

        public bool AddItem(Item item)
        {
            if (items.Find(i => i.ItemSO == item.ItemSO) is Item existing)
            {
                existing.UpdateCount(existing.Count + item.Count);
                return true;
            }

            items.Add(item);
            return true;
        }

        public bool RemoveItem(Item item)
        {
            return items.Remove(item);
        }
    }
}