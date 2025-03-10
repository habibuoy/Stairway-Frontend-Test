using Game.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class CraftableRecipeListItem : ListViewItem
    {

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI countText;

        protected override void OnSetData()
        {
            if (Data is not CraftableRecipeAvailability data) return;
            if (data.ItemRecipe == null
                || data.ItemRecipe.item == null) return;

            var item = data.ItemRecipe.item;

            image.sprite = item.Image;
            nameText.text = item.ItemName;

            bool isSufficient = data.GetAvailabilityCount() > 0;
            string style = isSufficient ? "<b>" : "";
            string styleEnd = isSufficient ? "</b>" : "";
            
            string color = isSufficient
                ? ColorExtensions.GetSufficientCraftStringColor()
                : ColorExtensions.GetInsufficientCraftStringColor();
            countText.text = $"<color={color}>{style}{data.AvailableAmount}</color>{styleEnd} / {data.ItemRecipe.count}";
        }
    }
}