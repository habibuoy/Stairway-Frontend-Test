using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class BackpackListViewItem : ListViewItem
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countText;

        protected override void OnSetData()
        {
            var data = Data as ItemData;

            image.sprite = data.Item.Image;
            countText.text = data.Item.Count.ToString();
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
        }
    }
}