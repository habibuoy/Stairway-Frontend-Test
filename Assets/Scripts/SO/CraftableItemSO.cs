using UnityEngine;

namespace Game.UI.SO
{
    [CreateAssetMenu(fileName = "CraftableItem", menuName = "Inventory/Craftable Item")]
    public class CraftableItemSO : ItemSO
    {
        [SerializeField] private ItemSO[] requirements;

        private void OnValidate()
        {
            itemName = Split(name);
        }
    }
}