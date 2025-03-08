using System;
using System.Collections.Generic;

namespace Game.Extensions
{
    public static class DateTimeExtensions
    {
        private static Dictionary<SeasonType, string> seasonStrings = new();

        public static SeasonType AsSeason(this DateTime dateTime)
        {
            float value = (float) dateTime.Month + dateTime.Day / 100f;
            if (value < 3.1 || value > 12.1) return SeasonType.Winter;
            if (value < 6.1) return SeasonType.Spring;
            if (value < 9.1) return SeasonType.Summer;

            return SeasonType.Autumn;
        }

        public static string AsString(this SeasonType seasonType)
        {
            if (!seasonStrings.TryGetValue(seasonType, out var season))
            {
                season = seasonStrings[seasonType] = seasonType.ToString();
            }

            return season;
        }

        public enum SeasonType
        {
            Spring,
            Summer,
            Autumn,
            Winter
        }
    }
}