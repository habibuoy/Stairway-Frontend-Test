using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.SO
{
    [CreateAssetMenu(fileName = "CraftableItem", menuName = "Inventory/Craftable Item")]
    public class CraftableItemSO : ItemSO
    {
        [SerializeField] private RecipeItem[] recipe;
        [SerializeField] private int space;
        [SerializeField] private int craftDuration;

        public IReadOnlyCollection<RecipeItem> Recipe => recipe;
        public int Space => space;
        public int CraftDuration => craftDuration;

        private void OnValidate()
        {
            itemName = Split(name);
        }
    }

    [Serializable]
    public class RecipeItem
    {
        public ItemSO item;
        public int count;
    }
}