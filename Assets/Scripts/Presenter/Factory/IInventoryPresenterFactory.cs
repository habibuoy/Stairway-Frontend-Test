using Game.UI.Model.Inventory;
using Game.UI.View.Inventory;

namespace Game.UI.Presenter.Inventory.Factory
{
    public interface IInventoryPresenterFactory
    {
        IInventoryPresenter Create<TModel, TView>(string path, TModel model, TView view)
            where TModel : BaseInventoryModel
            where TView : BaseInventoryView;
    }
}