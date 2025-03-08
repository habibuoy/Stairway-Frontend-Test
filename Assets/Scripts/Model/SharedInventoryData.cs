using System.Collections.Generic;
using Game.Interface.UI.Inventory;
using Game.UI.Model.Inventory;

namespace Game.UI.Inventory
{
    public class SharedInventoryData : ISharedInventoryData
    {
        private readonly List<Item> Items = new();

        public IReadOnlyList<Item> OwnedItems => Items;

        public SharedInventoryData(Item[] items)
        {
            Items.AddRange(items);
        }
    }
}