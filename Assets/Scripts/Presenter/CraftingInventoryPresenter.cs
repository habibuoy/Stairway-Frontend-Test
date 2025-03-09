using System.Collections.Generic;
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
            view.UpdateCraftableItems(model.Craftables.Select(item => 
            {
                var craftableSO = item.ItemSO as CraftableItemSO;
                List<CraftableRecipeAvailability> craftableRecipeAvailabilities = new();
                if (craftableSO.Recipe != null)
                {
                    foreach (var recipeItem in craftableSO.Recipe)
                    {
                        var availableItem = model.AllItems.FirstOrDefault(i => i.ItemSO == recipeItem.item);
                        int availableAmount = availableItem == null ? 0 : availableItem.Count;
                        craftableRecipeAvailabilities.Add(new CraftableRecipeAvailability(recipeItem, availableAmount));
                    }
                }

                var recipeData = new CraftableRecipeItemData(item, craftableRecipeAvailabilities);
                return recipeData;
            }).ToList<IListData>());
            view.BackpackItemClicked += OnBackpackItemClicked;
            view.CraftableItemClicked += OnCraftableItemClicked;

            view.SelectCraftableItem(0);
        }

        protected override void OnDestroy()
        {
            view.BackpackItemClicked -= OnBackpackItemClicked;
            view.CraftableItemClicked -= OnCraftableItemClicked;
        }

        private void OnBackpackItemClicked(ItemData itemData)
        {
            Debug.Log($"Item {itemData.Item.ItemName} category {itemData.Item.ItemCategory} count {itemData.Item.Count} clicked");
        }

        private void OnCraftableItemClicked(CraftableRecipeItemData itemData)
        {
            view.UpdateCraftableDetail(itemData);
        }
    }
}