using System.Collections.Generic;
using Game.UI.Model.Inventory;

namespace Game.Interface.UI.Inventory
{
    public interface ISharedInventoryData
    {
        IReadOnlyList<Item> OwnedItems { get; }
    }
}