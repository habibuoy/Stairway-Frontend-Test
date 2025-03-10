using UnityEngine;

namespace Game.Extensions
{
    public static class ColorExtensions
    {
        private static Color sufficientCraftColor = Color.black;
        private static Color insufficientCraftColor = Color.red;

        public static string GetSufficientCraftStringColor()
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(sufficientCraftColor)}";
        }

        public static string GetInsufficientCraftStringColor()
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(insufficientCraftColor)}";
        }
    }
}