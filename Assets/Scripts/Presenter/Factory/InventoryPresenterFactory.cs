using Game.UI.Model.Inventory;
using Game.UI.View.Inventory;

namespace Game.UI.Presenter.Inventory.Factory
{
    public class InventoryPresenterFactory : IInventoryPresenterFactory
    {
        IInventoryPresenter IInventoryPresenterFactory.Create<TModel, TView>(string path, TModel model, TView view)
        {
            IInventoryPresenter presenter;
            switch (path)
            {
                case "General":
                    presenter = new GeneralInventoryPresenter(model as GeneralInventoryModel, 
                        view as GeneralInventoryView);
                    break;
                case "Crafting":
                    presenter = new CraftingInventoryPresenter(model as CraftingInventoryModel, 
                        view as CraftingInventoryView);
                    break;
                default:
                    presenter = new OtherInventoryPresenter(model as OtherInventoryModel, 
                        view as OtherInventoryView);
                    break;
            }

            presenter?.Initialize();
            return presenter;
        }
    }
}