using Game.Extensions;
using TMPro;
using UnityEngine;

namespace Game.UI.View.Components
{
    public class CraftableHoverInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hoverInfoNameText;
        [SerializeField] private TextMeshProUGUI hoverOwnedCountText;
        [SerializeField] private TextMeshProUGUI hoverCraftabilityCountText;
        [SerializeField] private Vector2 showPositionOffset = new Vector2(0, -62f);

        public void ShowInfo(CraftableRecipeItemData itemData, Vector2 position)
        {
            hoverInfoNameText.text = itemData.Item.ItemName;
            hoverOwnedCountText.text = itemData.Item.Count.ToString();
            int craftabilityCount = itemData.GetCraftabilityCount();
            string color = craftabilityCount > 0 
                ? ColorExtensions.GetSufficientCraftStringColor()
                : ColorExtensions.GetInsufficientCraftStringColor();
            hoverCraftabilityCountText.text = $"<color={color}>{craftabilityCount}</color>";

            (transform as RectTransform).anchoredPosition = position + showPositionOffset;

            gameObject.SetActive(true);
        }

        public void HideInfo()
        {
            gameObject.SetActive(false);
        }
    }
}