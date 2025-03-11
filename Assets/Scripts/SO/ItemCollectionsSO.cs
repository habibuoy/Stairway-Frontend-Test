using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.SO
{
    [CreateAssetMenu(fileName = "ItemCollections", menuName = "Inventory/Item Collections")]
    public class ItemCollectionsSO : ScriptableObject
    {
        [SerializeField] private ItemSO[] items;
        [SerializeField] private CraftableItemSO[] craftables;

        public IReadOnlyCollection<ItemSO> Items => items;
        public IReadOnlyCollection<CraftableItemSO> Craftables => craftables;
    }
}