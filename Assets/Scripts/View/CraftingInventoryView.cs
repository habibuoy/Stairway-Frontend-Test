using UnityEngine;
using UnityEngine.EventSystems;
using Game.UI.View.Components;
using System.Collections.Generic;
using System;
using Game.UI.Model.Inventory;

namespace Game.UI.View.Inventory
{
    public class CraftingInventoryView : BaseInventoryView
    {
        [SerializeField] private ListView backpackItemlist;
        [SerializeField] private ListView craftableItemlist;
        [SerializeField] private CraftableItemDetailView craftableItemDetailView;
        [SerializeField] private CraftableHoverInfo craftableHoverInfo;
        [SerializeField] private TabView categoryTabView;
        [SerializeField] private KeyCode pinCraftableKeyCode;
        [SerializeField] private float craftMoreKeyHoldDuration = 1f;

        private ItemCategory currentCategory;
        private ListViewItem selectedCraftableItem;

        private bool isHolding;
        private float holdProgress;

        public event Action<ItemData> BackpackItemClicked;
        public event Action<CraftableRecipeItemData, bool> CraftableItemClicked;
        public event Action<CraftableRecipeItemData> CraftableItemBegunHover;
        public event Action<CraftableRecipeItemData> CraftableItemEndedHover;
        public event Action<ItemCategory> CategoryTabChanged;
        public event Action<CraftableRecipeItemData> CraftablePinInputted; 
        public event Action<CraftableRecipeItemData> CraftableItemHeld;

        public override void Initialize()
        {
            base.Initialize();
            backpackItemlist.ItemClicked += OnBackpackItemClicked;
            craftableItemlist.ItemClicked += OnCraftableItemClicked;
            craftableItemlist.ItemBegunHover += OnCraftableItemBegunHover;
            craftableItemlist.ItemEndedHover += OnCraftableItemEndedHover;
            craftableItemlist.ItemBegunClick += OnCraftableItemBegunClick;
            craftableItemlist.ItemEndedClick += OnCraftableItemEndedClick;
            categoryTabView.TabChanged += OnCategoryTabChanged;
            craftableHoverInfo.HideInfo();

            craftableItemlist.ToggleItemSelectable(true);
        }

        private void OnDestroy()
        {
            backpackItemlist.ItemClicked -= OnBackpackItemClicked;
            craftableItemlist.ItemClicked -= OnCraftableItemClicked;
            craftableItemlist.ItemBegunHover -= OnCraftableItemBegunHover;
            craftableItemlist.ItemEndedHover -= OnCraftableItemEndedHover;
            craftableItemlist.ItemBegunClick -= OnCraftableItemBegunClick;
            craftableItemlist.ItemEndedClick -= OnCraftableItemEndedClick;
            categoryTabView.TabChanged -= OnCategoryTabChanged;
        }

        private void Update()
        {
            if (Input.GetKeyUp(pinCraftableKeyCode))
            {
                var selected = craftableItemlist.GetListViewItem(item => item.IsSelected);
                if (selected == null) return;
                CraftablePinInputted?.Invoke(selected.Data as CraftableRecipeItemData);
            }

            if (isHolding
                && selectedCraftableItem != null)
            {
                holdProgress -= Time.deltaTime;
                if (holdProgress <= 0f)
                {
                    ResetHolding();
                    CraftableItemHeld?.Invoke(selectedCraftableItem.Data as CraftableRecipeItemData);
                }
            }
        }

        public void UpdateBackpackItems(List<IListData> datas)
        {
            backpackItemlist.UpdateList(datas, item => 
            {
                if (item.Data.IsBlankData())
                {
                    item.SetInteractable(false);
                    item.SetBlanked(true);
                }
            });
        }

        public void UpdateCraftableItems(List<IListData> datas)
        {
            craftableItemlist.UpdateList(datas, item => 
            {
                if (item.Data.IsBlankData())
                {
                    item.SetInteractable(false);
                    item.SetBlanked(true);
                }
            });
        }

        public void UpdateCraftableDetail(CraftableRecipeItemData craftableRecipeItemData)
        {
            craftableItemDetailView.SetData(craftableRecipeItemData);
        }

        public void ShowCraftableHoverInfo(CraftableRecipeItemData craftableRecipeItemData)
        {
            var listItem = craftableItemlist.GetListViewItem(li => li.Data == craftableRecipeItemData);
            if (listItem == null)
            {
                return;
            }

            craftableHoverInfo.ShowInfo(craftableRecipeItemData, listItem.RectTransform.anchoredPosition);
        }

        public void HideCraftableHoverInfo()
        {
            craftableHoverInfo.HideInfo();
        }

        public void SelectCraftableItem(int index)
        {
            craftableItemlist.SelectItem(index);
        }

        public void ReselectCraftableItem()
        {
            if (selectedCraftableItem != null)
            {
                craftableItemlist.SelectItem(selectedCraftableItem.Index);
            }
        }

        public void SortListByCategory(ItemCategory itemCategory)
        {
            if (currentCategory == itemCategory) return;
            currentCategory = itemCategory;

            craftableItemlist.SortItems(ListCategorySorter, CategoryIncludedListItemAction, 
                CategoryExcludedListItemAction);
            
            backpackItemlist.SortItems(ListCategorySorter, CategoryIncludedListItemAction, 
                CategoryExcludedListItemAction);
        }

        public void PinCraftableItem(CraftableRecipeItemData craftableRecipeItemData)
        {
            if (selectedCraftableItem == null
                || selectedCraftableItem.Data != craftableRecipeItemData) return;

            craftableItemlist.SetPinned(selectedCraftableItem.Index, !selectedCraftableItem.IsPinned);
        }

        private void ResetHolding()
        {
            isHolding = false;
            holdProgress = craftMoreKeyHoldDuration;
        }

        private bool ListCategorySorter(ListViewItem listItem)
        {
            if (listItem.Data is not ItemData data
                || data.IsBlankData()) return false;

            return currentCategory == ItemCategory.All || data.Item.ItemCategory == currentCategory;
        }

        private void CategoryIncludedListItemAction(ListViewItem listItem)
        {
            listItem.SetInteractable(true);
            listItem.SetBlanked(false);
        }

        private void CategoryExcludedListItemAction(ListViewItem listItem)
        {
            listItem.SetInteractable(false);
            listItem.SetBlanked();
        }

        private void OnBackpackItemClicked(ListViewItem listItem)
        {
            BackpackItemClicked?.Invoke(listItem.Data as ItemData);
        }

        private void OnCraftableItemClicked(ListViewItem listItem)
        {
            craftableItemlist.SetSelected(listItem.Index);
            CraftableItemClicked?.Invoke(listItem.Data as CraftableRecipeItemData,
                selectedCraftableItem == listItem);
            selectedCraftableItem = listItem;
        }

        private void OnCraftableItemBegunHover(ListViewItem listItem)
        {
            craftableItemlist.SetHovered(listItem.Index);
            CraftableItemBegunHover?.Invoke(listItem.Data as CraftableRecipeItemData);
        }

        private void OnCraftableItemEndedHover(ListViewItem listItem)
        {
            var data = listItem.Data as CraftableRecipeItemData;
            craftableItemlist.SetHovered(listItem.Index, false);
            CraftableItemEndedHover?.Invoke(data);
            if (isHolding 
                && selectedCraftableItem != null 
                && selectedCraftableItem.Data == data)
            {
                ResetHolding();
            }
        }

        private void OnCraftableItemBegunClick(ListViewItem listItem, ClickData clickData)
        {
            var data = listItem.Data as CraftableRecipeItemData;
            if (selectedCraftableItem != null
                && selectedCraftableItem.Data == data
                && clickData.Button == PointerEventData.InputButton.Right)
            {
                ResetHolding();
                isHolding = true;
            }
        }

        private void OnCraftableItemEndedClick(ListViewItem listItem, ClickData clickData)
        {
            var data = listItem.Data as CraftableRecipeItemData;
            if (isHolding 
                && selectedCraftableItem != null 
                && selectedCraftableItem.Data == data
                && clickData.Button == PointerEventData.InputButton.Right)
            {
                ResetHolding();
            }
        }

        private void OnCategoryTabChanged(string path)
        {
            if (Enum.TryParse<ItemCategory>(path, out var result))
            {
                selectedCraftableItem = null;
                CategoryTabChanged?.Invoke(result);
            }
        }
    }
}