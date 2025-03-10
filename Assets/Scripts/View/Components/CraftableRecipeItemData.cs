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
                    || data.ItemRequirement == null
                    || data.ItemRequirement.item == null)
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
        public RecipeItem ItemRequirement { get; private set; }
        public int AvailableAmount { get; private set; }

        private int availabilityCount;

        public CraftableRecipeAvailability(RecipeItem item, int amount)
        {
            ItemRequirement = item;
            AvailableAmount = amount;
            UpdateAvailabilityCount();
        }

        public bool IsBlankData()
        {
            return ItemRequirement.item == null;
        }

        private void UpdateAvailabilityCount()
        {
            availabilityCount = Mathf.FloorToInt(AvailableAmount / ItemRequirement.count);
        }

        public int GetAvailabilityCount()
        {
            return availabilityCount;
        }
    }
}