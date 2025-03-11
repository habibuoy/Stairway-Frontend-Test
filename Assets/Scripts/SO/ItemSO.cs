using Game.UI.Model.Inventory;
using UnityEngine;

namespace Game.UI.SO
{
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] protected string itemName;
        [SerializeField, TextArea(3, 6)] protected string description;
        [SerializeField] protected Sprite image;
        [SerializeField] protected ItemCategory category;

        public string ItemName => itemName;
        public string Description => description;
        public Sprite Image => image;
        public ItemCategory ItemCategory => category;

        private void OnValidate()
        {
            itemName = Split(name);
        }

        protected string Split(string input)
        {
            string result = "";
            
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    result += ' ';
                }
            
                result += input[i];
            }
            
            return result.Trim();
        }
    }
}