using System;
using System.Collections.Generic;
using System.Linq;
using Game.Interface.UI.Inventory;
using Game.UI.SO;

namespace Game.UI.Model.Inventory
{
    public class CraftingInventoryModel : BaseInventoryModel
    {
        public IEnumerable<Item> Craftables => sharedData.OwnedItems.Where(i => i.IsBlank() || i.ItemSO is CraftableItemSO);
        public IEnumerable<Item> Basics => sharedData.OwnedItems.Where(i => i.IsBlank() || i.ItemSO is not CraftableItemSO);
        public IEnumerable<Item> AllItems => sharedData.OwnedItems;

        public event Action<Item> ItemChanged;
        public event Action ItemsChanged;

        public CraftingInventoryModel(ISharedInventoryData sharedInventoryData) 
            : base(sharedInventoryData)
        {
        }

        public void AddCraftableItem(Item item)
        {
            if (sharedData.AddItem(item))
            {
                ItemsChanged?.Invoke();
            }
        }

        public void UpdateBasicItem(Item item, int count)
        {
            foreach (var basicItem in Basics)
            {
                if (basicItem.IsBlank()) continue;

                if (item == basicItem)
                {
                    basicItem.UpdateCount(count);
                    ItemChanged?.Invoke(item);
                    return;
                }
            }
        }

        public void RemoveBasicItem(Item item)
        {
            if (sharedData.RemoveItem(item))
            {
                ItemsChanged?.Invoke();
            }
        }
    }
}