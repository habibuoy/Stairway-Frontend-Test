using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.UI.Model.Inventory;
using Game.UI.SO;
using Game.UI.View.Components;
using Game.UI.View.Inventory;

namespace Game.UI.Presenter.Inventory
{
    public class CraftingInventoryPresenter : BaseInventoryPresenter<CraftingInventoryModel, CraftingInventoryView>
    {
        public event Action Hidden;

        public CraftingInventoryPresenter(CraftingInventoryModel model, CraftingInventoryView view) 
            : base(model, view) { }

        protected override void OnInitialize()
        {
            model.ItemChanged += OnModelItemChanged;
            model.ItemsChanged += OnModelItemsChanged;
            view.UpdateBackpackItems(GetBasicItemDatas());
            view.UpdateCraftableItems(GetCraftableItemDatas());
            view.BackpackItemClicked += OnBackpackItemClicked;
            view.CraftableItemClicked += OnCraftableItemClicked;
            view.CraftableItemBegunHover += OnCraftableBegunHovered;
            view.CraftableItemEndedHover += OnCraftableEndedHovered;
            view.CategoryTabChanged += OnCategoryTabChanged;
            view.CraftablePinInputted += OnCraftableItemInputtedPin;
            view.CraftableItemHeld += OnCraftableItemHeld;
            view.BackInputted += OnBackInputted;

            view.SelectCraftableItem(0);
        }

        protected override void OnDestroy()
        {
            model.ItemChanged -= OnModelItemChanged;
            model.ItemsChanged -= OnModelItemsChanged;
            view.BackpackItemClicked -= OnBackpackItemClicked;
            view.CraftableItemClicked -= OnCraftableItemClicked;
            view.CraftableItemBegunHover -= OnCraftableBegunHovered;
            view.CraftableItemEndedHover -= OnCraftableEndedHovered;
            view.CategoryTabChanged -= OnCategoryTabChanged;
            view.CraftablePinInputted -= OnCraftableItemInputtedPin;
            view.CraftableItemHeld -= OnCraftableItemHeld;
            view.BackInputted -= OnBackInputted;
        }

        private void OnModelItemChanged(Item item)
        {
            
        }

        private void OnModelItemsChanged()
        {
            
        }

        private void OnBackpackItemClicked(ItemData itemData)
        {
            
        }

        private void OnCraftableItemClicked(CraftableRecipeItemData itemData, bool currentlySelected)
        {
            if (currentlySelected)
            {
                CraftItem(itemData);
            }
            else
            {
                view.HideCraftableHoverInfo();
                view.UpdateCraftableDetail(itemData);
            }
        }
        
        private void CraftItem(CraftableRecipeItemData itemData, bool more = false)
        {
            int craftabilityCount = itemData.GetCraftabilityCount();
            if (craftabilityCount <= 0) return;

            var craftableItem = new Item(itemData.Item.ItemSO, !more ? 1 : craftabilityCount);
            foreach (var availability in itemData.AvailabilityData)
            {
                var recipeItem = model.AllItems.FirstOrDefault(item => 
                    item.ItemSO == availability.ItemRecipe.item);

                if (recipeItem == null)
                {
                    continue;
                }

                int craftRemainder = !more ? availability.AvailableAmount - availability.ItemRecipe.count 
                    : availability.AvailableAmount % availability.ItemRecipe.count;

                if (craftRemainder == 0)
                {
                    model.RemoveBasicItem(recipeItem);
                }
                else
                {
                    model.UpdateBasicItem(recipeItem, craftRemainder);
                }
            }
            model.AddCraftableItem(craftableItem);
            view.UpdateBackpackItems(GetBasicItemDatas());
            view.UpdateCraftableItems(GetCraftableItemDatas());
            view.HideCraftableHoverInfo();
        }

        private void OnCraftableBegunHovered(CraftableRecipeItemData itemData)
        {
            view.ShowCraftableHoverInfo(itemData);
        }

        private void OnCraftableEndedHovered(CraftableRecipeItemData itemData)
        {
            view.HideCraftableHoverInfo();
        }

        private void OnCraftableItemHeld(CraftableRecipeItemData itemData)
        {
            CraftItem(itemData, true);
        }

        private void OnCategoryTabChanged(ItemCategory category)
        {
            view.SortListByCategory(category);
            view.HideCraftableHoverInfo();
            view.SelectCraftableItem(0);
        }

        private void OnCraftableItemInputtedPin(CraftableRecipeItemData craftableRecipeItemData)
        {
            view.PinCraftableItem(craftableRecipeItemData);
        }

        private void OnBackInputted()
        {
            _ = Back();
        }

        private async Task Back()
        {
            await Hide();
            Hidden?.Invoke();
        }

        private List<IListData> GetBasicItemDatas(Func<Item, bool> match = null)
        {
            var basics = model.AllItems;
            if (match != null)
            {
                basics = basics.Where(match);
            }
            var datas = basics.Select(i => new ItemData(i)).ToList();
            datas.Sort();
            return datas.ToList<IListData>();
        }

        private List<IListData> GetCraftableItemDatas(Func<Item, bool> match = null)
        {
            var craftables = model.Craftables;
            if (match != null)
            {
                craftables = craftables.Where(match);
            }

            var craftableItemDatas = craftables.Select(item => 
            {
                List<CraftableRecipeAvailability> craftableRecipeAvailabilities = null;

                if (!item.IsBlank() && item.ItemSO is CraftableItemSO)
                {
                    var craftableSO = item.ItemSO as CraftableItemSO;
                    craftableRecipeAvailabilities = new();
                    if (craftableSO.Recipe != null)
                    {
                        foreach (var recipeItem in craftableSO.Recipe)
                        {
                            var availableItem = model.AllItems.FirstOrDefault(i => i.ItemSO == recipeItem.item);
                            int availableAmount = availableItem == null ? 0 : availableItem.Count;
                            craftableRecipeAvailabilities.Add(new CraftableRecipeAvailability(recipeItem, availableAmount));
                        }
                    }
                }

                var recipeData = new CraftableRecipeItemData(item, craftableRecipeAvailabilities);
                return recipeData;
            });

            var datas = craftableItemDatas.ToList();
            datas.Sort();

            return datas.ToList<IListData>();
        }
    }
}