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

        public event Action<Item> BackpackItemClicked;

        public override void Initialize()
        {
            base.Initialize();
            backpackItemlist.ItemClicked += OnBackpackItemClicked;
        }

        private void OnDestroy()
        {
            backpackItemlist.ItemClicked -= OnBackpackItemClicked;
        }

        public void UpdateBackpackItems(List<IListData> datas)
        {
            backpackItemlist.UpdateList(datas);
        }

        private void OnBackpackItemClicked(IListData data)
        {
            BackpackItemClicked?.Invoke((data as ItemData).Item);
        }
    }
}