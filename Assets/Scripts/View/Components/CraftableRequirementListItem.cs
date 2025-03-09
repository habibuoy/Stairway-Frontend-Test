using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class CraftableRequirementListItem : ListViewItem
    {
        private static Color sufficeColor = Color.black;
        private static Color insufficientColor = Color.red;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI countText;

        protected override void OnSetData()
        {
            if (Data is not CraftableItemRequirementData data) return;
            if (data.Requirement == null
                || data.Requirement.item == null) return;

            image.sprite = data.Requirement.item.Image;
            nameText.text = data.Requirement.item.ItemName;

            bool isSufficient = data.IsSufficient();
            string style = isSufficient ? "<b>" : "";
            string styleEnd = isSufficient ? "</b>" : "";
            
            string color = ColorUtility.ToHtmlStringRGBA(isSufficient
                ? sufficeColor 
                : insufficientColor);
            countText.text = $"<color=#{color}>{style}{data.AvailableAmount}</color>{styleEnd} / {data.Requirement.count}";
        }
    }
}