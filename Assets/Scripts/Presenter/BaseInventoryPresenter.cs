using Game.UI.Model.Inventory;
using Game.UI.View.Inventory;

namespace Game.UI.Presenter.Inventory
{
    public abstract class BaseInventoryPresenter<TModel, TView>
        where TModel : BaseInventoryModel
        where TView : BaseInventoryView
    {
        protected TModel model;
        protected TView view;

        public BaseInventoryPresenter(BaseInventoryModel model, BaseInventoryView view)
        {
            this.model = (TModel) model;
            this.view = (TView) view;
        }

        public void Initialize()
        {
            view.Initialize();
            OnInitialize();
        }

        public void Destroy()
        {
            OnDestroy();
        }
        
        protected abstract void OnInitialize();
        protected abstract void OnDestroy();
    }
}