using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class BackpackListViewItem : ListViewItem
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countText;

        protected override void OnSetData()
        {
            var data = Data as ItemData;

            image.sprite = data.Item.Image;
            countText.text = data.Item.Count.ToString();
            UpdateBackgroundColor();
            image.gameObject.SetActive(true);
            countText.transform.parent.gameObject.SetActive(true);
        }

        protected override void OnSetBlanked()
        {
            base.OnSetBlanked();
            if (IsBlanked)
            {
                image.gameObject.SetActive(false);
                countText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                image.gameObject.SetActive(true);
                countText.transform.parent.gameObject.SetActive(true);
            }
            UpdateBackgroundColor();
        }

        private void UpdateBackgroundColor()
        {
            var color = backgroundImage.color;
            color.a = IsBlanked ? 0.25f : 1f;
            backgroundImage.color = color;
        }
    }
}