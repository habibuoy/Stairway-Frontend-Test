using Game.UI.Model.Inventory;
using Game.UI.View.Inventory;

namespace Game.UI.Presenter.Inventory
{
    public class OtherInventoryPresenter : BaseInventoryPresenter<OtherInventoryModel, OtherInventoryView>
    {
        public OtherInventoryPresenter(OtherInventoryModel model, OtherInventoryView view) : base(model, view)
        {
        }

        protected override void OnDestroy()
        {
            
        }

        protected override void OnInitialize()
        {
            
        }
    }
}