using UnityEngine;
using Game.UI.View.Components;
using System.Collections.Generic;
using Game.UI.Model.Inventory;
using System;

namespace Game.UI.View.Inventory
{
    public class CraftingInventoryView : BaseInventoryView
    {
        [SerializeField] private ListView backpackItemlist;
        [SerializeField] private ListView craftableItemlist;

        public event Action<Item> BackpackItemClicked;
        public event Action<Item> CraftableItemClicked;

        public override void Initialize()
        {
            base.Initialize();
            backpackItemlist.ItemClicked += OnBackpackItemClicked;
            craftableItemlist.ItemClicked += OnCraftableItemClicked;

            craftableItemlist.ToggleItemSelectable(true);
        }

        private void OnDestroy()
        {
            backpackItemlist.ItemClicked -= OnBackpackItemClicked;
            craftableItemlist.ItemClicked -= OnCraftableItemClicked;
        }

        public void UpdateBackpackItems(List<IListData> datas)
        {
            backpackItemlist.UpdateList(datas);
        }

        public void UpdateCraftableItems(List<IListData> datas)
        {
            craftableItemlist.UpdateList(datas);
        }

        public void SelectCraftableItem(int index)
        {
            craftableItemlist.SetSelected(index);
        }

        private void OnBackpackItemClicked(ListViewItem listItem)
        {
            BackpackItemClicked?.Invoke((listItem.Data as ItemData).Item);
        }

        private void OnCraftableItemClicked(ListViewItem listItem)
        {
            craftableItemlist.SetSelected(listItem.Index);
            CraftableItemClicked?.Invoke((listItem.Data as ItemData).Item);
        }
    }
}