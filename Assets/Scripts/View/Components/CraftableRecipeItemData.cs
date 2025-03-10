using System.Collections.Generic;
using System.Linq;
using Game.UI.Model.Inventory;
using Game.UI.SO;
using UnityEngine;

namespace Game.UI.View.Components
{
    public class CraftableRecipeItemData : ItemData
    {
        public List<CraftableRecipeAvailability> AvailabilityData { get; private set; }

        private int craftAbilityCount;

        public CraftableRecipeItemData(Item item, 
            List<CraftableRecipeAvailability> availability) : base(item)
        {
            AvailabilityData = availability;
            UpdateCraftabilityCount();
        }

        private void UpdateCraftabilityCount()
        {
            if (AvailabilityData == null)
            {
                craftAbilityCount = 0;
                return;
            } 
            int[] availabilities = new int[AvailabilityData.Count];
            for (int i = 0; i < availabilities.Length; i++)
            {
                var data = AvailabilityData[i];
                if (data == null
                    || data.ItemRecipe == null
                    || data.ItemRecipe.item == null)
                {
                    // immediately return if there is error
                    return;
                }

                availabilities[i] = data.GetAvailabilityCount();
            }

            craftAbilityCount = availabilities.Min();
        }

        public int GetCraftabilityCount()
        {
            return craftAbilityCount;
        }

        public bool IsCraftable()
        {
            return GetCraftabilityCount() > 0;
        }
    }

    public class CraftableRecipeAvailability : IListData
    {
        public RecipeItem ItemRecipe { get; private set; }
        public int AvailableAmount { get; private set; }

        private int availabilityCount;

        public CraftableRecipeAvailability(RecipeItem item, int amount)
        {
            ItemRecipe = item;
            AvailableAmount = amount;
            UpdateAvailabilityCount();
        }

        public bool IsBlankData()
        {
            return ItemRecipe.item == null;
        }

        private void UpdateAvailabilityCount()
        {
            availabilityCount = Mathf.FloorToInt(AvailableAmount / ItemRecipe.count);
        }

        public int GetAvailabilityCount()
        {
            return availabilityCount;
        }

        public int CompareTo(IListData other)
        {
            return ItemRecipe.item.ItemName.CompareTo((other as CraftableRecipeItemData).Item.ItemName);
        }
    }
}