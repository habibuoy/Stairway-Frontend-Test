using System;
using Game.UI.Model.Inventory;

namespace Game.UI.View.Components
{
    public class ItemData : IListData, IComparable<ItemData>
    {
        public Item Item { get; private set; }

        public virtual int Id => Item.IsBlank() ? -1 : Item.ItemSO.GetHashCode();

        public virtual bool IsBlankData()
        {
            return Item.IsBlank();
        }

        public int CompareTo(ItemData other)
        {
            if (IsBlankData())
            {
                if (other.IsBlankData())
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (other.IsBlankData())
                {
                    return -1;
                }
                else
                {
                    return Item.ItemName.CompareTo(other.Item.ItemName);
                }
            }
        }

        int IComparable<IListData>.CompareTo(IListData other)
        {
            if (other is ItemData itemData)
            {
                return CompareTo(itemData);
            }

            return 0;
        }

        public ItemData(Item item)
        {
            Item = item;
        }
    }
}