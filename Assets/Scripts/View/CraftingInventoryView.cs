using UnityEngine;
using Game.UI.View.Components;
using System.Collections.Generic;
using System;

namespace Game.UI.View.Inventory
{
    public class CraftingInventoryView : BaseInventoryView
    {
        [SerializeField] private ListView backpackItemlist;
        [SerializeField] private ListView craftableItemlist;
        [SerializeField] private CraftableItemDetailView craftableItemDetailView;
        [SerializeField] private CraftableHoverInfo craftableHoverInfo;

        public event Action<ItemData> BackpackItemClicked;
        public event Action<CraftableRecipeItemData> CraftableItemClicked;
        public event Action<CraftableRecipeItemData> CraftableItemBegunHover;
        public event Action<CraftableRecipeItemData> CraftableItemEndedHover;

        public override void Initialize()
        {
            base.Initialize();
            backpackItemlist.ItemClicked += OnBackpackItemClicked;
            craftableItemlist.ItemClicked += OnCraftableItemClicked;
            craftableItemlist.ItemBegunHover += OnCraftableItemBegunHover;
            craftableItemlist.ItemEndedHover += OnCraftableItemEndedHover;
            craftableHoverInfo.HideInfo();

            craftableItemlist.ToggleItemSelectable(true);
        }

        private void OnDestroy()
        {
            backpackItemlist.ItemClicked -= OnBackpackItemClicked;
            craftableItemlist.ItemClicked -= OnCraftableItemClicked;
            craftableItemlist.ItemBegunHover -= OnCraftableItemBegunHover;
            craftableItemlist.ItemEndedHover -= OnCraftableItemEndedHover;
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

        private void OnBackpackItemClicked(ListViewItem listItem)
        {
            BackpackItemClicked?.Invoke(listItem.Data as ItemData);
        }

        private void OnCraftableItemClicked(ListViewItem listItem)
        {
            craftableItemlist.SetSelected(listItem.Index);
            CraftableItemClicked?.Invoke(listItem.Data as CraftableRecipeItemData);
        }

        private void OnCraftableItemBegunHover(ListViewItem listItem)
        {
            craftableItemlist.SetHovered(listItem.Index);
            CraftableItemBegunHover?.Invoke(listItem.Data as CraftableRecipeItemData);
        }

        private void OnCraftableItemEndedHover(ListViewItem listItem)
        {
            craftableItemlist.SetHovered(listItem.Index, false);
            CraftableItemEndedHover?.Invoke(listItem.Data as CraftableRecipeItemData);
        }
    }
}