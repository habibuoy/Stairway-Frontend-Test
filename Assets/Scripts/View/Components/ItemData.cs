using Game.UI.Model.Inventory;

namespace Game.UI.View.Components
{
    public class ItemData : IListData
    {
        public Item Item { get; private set; }

        public ItemData(Item item)
        {
            Item = item;
        }
    }
}