using Game.Interface.UI.Inventory;
using UnityEngine;

namespace Game.UI.Model.Inventory.factory
{
    public class InventoryModelFactory : IInventoryModelFactory
    {
        private readonly CharacterModel characterModel;
        private readonly ISharedInventoryData sharedInventoryData;
        private readonly int gold;

        public InventoryModelFactory(CharacterModel characterModel, 
            ISharedInventoryData sharedInventoryData, int gold)
        {
            this.characterModel = characterModel;
            this.sharedInventoryData = sharedInventoryData;
            this.gold = gold;
        }

        BaseInventoryModel IInventoryModelFactory.Create(string path)
        {
            if (characterModel == null
                || sharedInventoryData == null)
            {
                Debug.LogError($"Character model or shared inventory data is not assigned!");
                return null;
            }

            switch (path)
            {
                case "General":
                    return new GeneralInventoryModel(sharedInventoryData,
                        characterModel, gold);
                case "Crafting":
                    return new CraftingInventoryModel(sharedInventoryData);
                default:
                    return new OtherInventoryModel(sharedInventoryData);
            }
        }
    }
}