using System;
using Game.Interface.UI.Inventory;
using UnityEngine;

namespace Game.UI.Model.Inventory
{
    public sealed class GeneralInventoryModel : BaseInventoryModel
    {
        private const int MaxGold = 100000;

        public CharacterModel CharacterModel { get; private set; }

        public int GoldAmount { get; private set; }

        public event Action<int> GoldChanged;

        public GeneralInventoryModel(ISharedInventoryData inventoryData, CharacterModel characterModel, int gold)
            : base(inventoryData)
        {
            CharacterModel = characterModel;
            GoldAmount = gold;
        }

        public void IncreaseGold(int amount)
        {
            GoldAmount = Mathf.Clamp(GoldAmount + amount, 0, MaxGold);
            GoldChanged?.Invoke(GoldAmount);
        }

        public void DecreaseGold(int amount)
        {
            GoldAmount = Mathf.Clamp(GoldAmount - amount, 0, MaxGold);
            GoldChanged?.Invoke(GoldAmount);
        }
    }
}