using System.Collections;
using System.Linq;
using Game.UI.Model.Inventory;
using Game.UI.SO;
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
            view.UpdateCraftableItems(model.Craftables.Select(i => new ItemData(i)).ToList<IListData>());
            view.BackpackItemClicked += OnBackpackItemClicked;
            view.CraftableItemClicked += OnCraftableItemClicked;

            view.SelectCraftableItem(0);
        }

        protected override void OnDestroy()
        {
            view.BackpackItemClicked -= OnBackpackItemClicked;
            view.CraftableItemClicked -= OnCraftableItemClicked;
        }

        private void OnBackpackItemClicked(Item item)
        {
            Debug.Log($"Item {item.ItemName} category {item.ItemCategory} count {item.Count} clicked");
        }

        private void OnCraftableItemClicked(Item item)
        {
            var craftableSO = item.ItemSO as CraftableItemSO;

            view.UpdateCraftableDetail(item, craftableSO.Requirements == null 
                ? Enumerable.Empty<CraftableItemRequirementData>() 
                : craftableSO.Requirements.Select(ir => 
                    {
                        var availableItem = model.Basics.FirstOrDefault(i => i.ItemSO == ir.item);
                        int availableAmount = availableItem == null ? 0 : availableItem.Count;
                        return new CraftableItemRequirementData(item, ir, availableAmount);
                    }));
        }
    }
}