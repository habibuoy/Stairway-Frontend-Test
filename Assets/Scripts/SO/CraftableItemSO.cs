using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.SO
{
    [CreateAssetMenu(fileName = "CraftableItem", menuName = "Inventory/Craftable Item")]
    public class CraftableItemSO : ItemSO
    {
        [SerializeField] private CraftableItemRequirement[] requirements;

        public IReadOnlyCollection<CraftableItemRequirement> Requirements => requirements;

        private void OnValidate()
        {
            itemName = Split(name);
        }
    }

    [Serializable]
    public class CraftableItemRequirement
    {
        public ItemSO item;
        public int count;
    }
}