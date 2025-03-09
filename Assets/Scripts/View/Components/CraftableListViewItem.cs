using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class CraftableListViewItem : ListViewItem
    {
        [SerializeField] private Image highlightImage;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image craftableImage;
        [SerializeField] private Image pinIconImage;

        protected override void OnSetData()
        {
            var data = Data as CraftableRecipeItemData;
            
            itemImage.sprite = data.Item.Image;
            highlightImage.gameObject.SetActive(false);
            craftableImage.gameObject.SetActive(false);
            pinIconImage.gameObject.SetActive(false);
        }

        protected override void OnSetSelected()
        {
            highlightImage.gameObject.SetActive(IsSelected);
        }
    }
}