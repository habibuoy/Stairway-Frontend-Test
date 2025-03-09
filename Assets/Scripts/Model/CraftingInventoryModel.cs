using System.Collections.Generic;
using System.Linq;
using Game.Interface.UI.Inventory;
using Game.UI.SO;

namespace Game.UI.Model.Inventory
{
    public class CraftingInventoryModel : BaseInventoryModel
    {
        public IEnumerable<Item> Craftables => sharedData.OwnedItems.Where(i => i.ItemSO is CraftableItemSO);
        public IEnumerable<Item> Basics => sharedData.OwnedItems.Where(i => i.ItemSO is not CraftableItemSO);
        public IEnumerable<Item> AllItems => sharedData.OwnedItems;

        public CraftingInventoryModel(ISharedInventoryData sharedInventoryData) 
            : base(sharedInventoryData)
        {
        }
    }
}