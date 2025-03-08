using UnityEngine;

namespace Game.UI
{
    public class InventoryTabButton : TabButton
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;

        public override void ToggleActive(bool active)
        {
            base.ToggleActive(active);
            buttonImage.color = active ? activeColor : inactiveColor;
            nameText.gameObject.SetActive(active);
        }
    }
}