namespace Game.UI.Model.Inventory.factory
{
    public interface IInventoryModelFactory
    {
        BaseInventoryModel Create(string path);
    }
}