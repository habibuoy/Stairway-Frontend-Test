using Game.Interface.UI.Inventory;

namespace Game.UI.Model.Inventory
{
    public abstract class BaseInventoryModel
    {
        protected ISharedInventoryData sharedData;

        public BaseInventoryModel(ISharedInventoryData sharedInventoryData)
        {
            sharedData = sharedInventoryData;
        }
    }
}