using System.Collections.Generic;
using Game.UI.Model.Inventory;

namespace Game.Extensions
{
    public static class ItemCategoryExtensions
    {
        private static Dictionary<ItemCategory, string> categoryCache = new();
        
        public static string AsString(this ItemCategory itemCategory)
        {
            if (!categoryCache.TryGetValue(itemCategory, out var category))
            {
                category = categoryCache[itemCategory] = itemCategory.ToString();
            }

            return category;
        }
    }
}