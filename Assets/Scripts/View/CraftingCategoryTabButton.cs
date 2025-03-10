using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View
{
    public class CraftingCategoryTabButton : TabButton
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private float activeXPositionOffset = 42;

        public override void OnToggleActive()
        {
            base.OnToggleActive();

            iconImage.rectTransform.anchoredPosition = IsActive 
                ? new Vector2(-activeXPositionOffset, iconImage.rectTransform.anchoredPosition.y) 
                : Vector2.zero;
            nameText.rectTransform.anchoredPosition = IsActive 
                ? new Vector2(activeXPositionOffset, nameText.rectTransform.anchoredPosition.y) 
                : Vector2.zero;
            nameText.gameObject.SetActive(IsActive);
        }

        public override void OnEndedHover()
        {
            if (IsActive) return;
            base.OnEndedHover();
        }
    }
}