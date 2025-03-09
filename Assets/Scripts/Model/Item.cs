using Game.UI.SO;
using UnityEngine;

namespace Game.UI.Model.Inventory
{
    public class Item
    {
        public ItemSO ItemSO { get; private set; }
        public int Count { get; private set; }

        public string ItemName => ItemSO.ItemName;
        public string ItemDescription => ItemSO.Description;
        public Sprite Image => ItemSO.Image;
        public ItemCategory ItemCategory => ItemSO.ItemCategory;

        public Item(ItemSO itemSO, int count)
        {
            ItemSO = itemSO;
            Count = count;
        }
    }
}