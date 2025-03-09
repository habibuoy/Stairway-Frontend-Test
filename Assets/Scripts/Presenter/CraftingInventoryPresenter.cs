using System.Linq;
using Game.UI.Model.Inventory;
using Game.UI.View.Components;
using Game.UI.View.Inventory;
using UnityEngine;

namespace Game.UI.Presenter.Inventory
{
    public class CraftingInventoryPresenter : BaseInventoryPresenter<CraftingInventoryModel, CraftingInventoryView>
    {
        public CraftingInventoryPresenter(CraftingInventoryModel model, CraftingInventoryView view) 
            : base(model, view) { }

        protected override void OnInitialize()
        {
            view.UpdateBackpackItems(model.Basics.Select(i => new ItemData(i)).ToList<IListData>());
            view.BackpackItemClicked += OnBackpackItemClicked;
        }

        protected override void OnDestroy()
        {
            view.BackpackItemClicked -= OnBackpackItemClicked;
        }

        private void OnBackpackItemClicked(Item item)
        {
            Debug.Log($"Item {item.ItemName} category {item.ItemCategory} count {item.Count} clicked");
        }
    }
}