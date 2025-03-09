using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class BackpackListViewItem : ListViewItem
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countText;

        public override void OnSetData()
        {
            var data = Data as ItemData;
            
            image.sprite = data.Item.Image;
            countText.text = data.Item.Count.ToString();
        }
    }
}