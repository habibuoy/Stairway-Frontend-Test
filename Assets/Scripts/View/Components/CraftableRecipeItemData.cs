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

        public CraftableRecipeItemData(Item item, 
            List<CraftableRecipeAvailability> availability) : base(item)
        {
            AvailabilityData = availability;
        }

        public int GetCraftabilityCount()
        {
            int[] availabilities = new int[AvailabilityData.Count];
            for (int i = 0; i < availabilities.Length; i++)
            {
                var data = AvailabilityData[i];
                if (data == null
                    || data.ItemRequirement == null
                    || data.ItemRequirement.item == null)
                {
                    // immediately return 0 if there is error
                    return 0;
                }

                availabilities[i] = data.GetAvailabilityCount();
            }

            return availabilities.Min();
        }
    }

    public class CraftableRecipeAvailability : IListData
    {
        public RecipeItem ItemRequirement { get; private set; }
        public int AvailableAmount { get; private set; }

        public CraftableRecipeAvailability(RecipeItem item, int amount)
        {
            ItemRequirement = item;
            AvailableAmount = amount;
        }

        public int GetAvailabilityCount()
        {
            return Mathf.FloorToInt(AvailableAmount / ItemRequirement.count);
        }
    }
}