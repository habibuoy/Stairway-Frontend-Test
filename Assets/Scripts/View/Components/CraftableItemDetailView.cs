using System.Collections.Generic;
using System.Linq;
using Game.Extensions;
using Game.UI.Model.Inventory;
using Game.UI.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class CraftableItemDetailView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI categoryText;
        [SerializeField] private TextMeshProUGUI spaceText;
        [SerializeField] private TextMeshProUGUI craftDurationText;
        [SerializeField] private ListView ingredientList;

        public void SetData(CraftableRecipeItemData craftableRecipeItemData)
        {
            var item = craftableRecipeItemData.Item;
            if (item.IsBlank()) return;
            var availabilities = craftableRecipeItemData.AvailabilityData;
            
            if (item.ItemSO is not CraftableItemSO craftableItem) return;

            nameText.text = item.ItemName;
            image.sprite = item.Image;
            descriptionText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(item.ItemDescription));
            descriptionText.text = item.ItemDescription;
            categoryText.text = item.ItemCategory.AsString();
            bool pluralSpace = craftableItem.Space > 1;
            spaceText.text = $"<color={ColorExtensions.GetBlackStringColor()}> "
                + $"{craftableItem.Space}</color> Space{(pluralSpace ? "s" : "")}";
            bool pluralHour = craftableItem.CraftDuration > 1;
            craftDurationText.text = $"<color={ColorExtensions.GetBlackStringColor()}> " + 
                $"{craftableItem.CraftDuration}</color> Hour{(pluralHour ? "s" : "")}";

            if (availabilities == Enumerable.Empty<CraftableRecipeItemData>())
            {
                ingredientList.ClearList();
                return;
            }

            ingredientList.UpdateList(availabilities.ToList<IListData>());
        }
    }
}