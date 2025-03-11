using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.SO
{
    [CreateAssetMenu(fileName = "CraftableItem", menuName = "Inventory/Craftable Item")]
    public class CraftableItemSO : ItemSO
    {
        [SerializeField] private RecipeItem[] recipe;

        public IReadOnlyCollection<RecipeItem> Recipe => recipe;

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