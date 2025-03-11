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

        public void UpdateCount(int count)
        {
            Count = count;
        }

        public bool IsBlank()
        {
            return ItemSO == null && Count == -1;
        }

        public static Item Blank()
        {
            return new Item(null, -1);
        }
    }
}