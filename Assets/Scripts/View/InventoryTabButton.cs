using UnityEngine;

namespace Game.UI
{
    public class InventoryTabButton : TabButton
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;

        public override void OnToggleActive()
        {
            buttonImage.color = IsActive ? activeColor : inactiveColor;
            nameText.gameObject.SetActive(IsActive);
        }

        public override void OnEndedHover()
        {
            buttonImage.color = buttonImage.color = IsActive ? activeColor : inactiveColor;
        }
    }
}