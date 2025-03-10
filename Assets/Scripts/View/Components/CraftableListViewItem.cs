using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    public class CraftableListViewItem : ListViewItem
    {
        [SerializeField] private Image highlightImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image placeholderImage;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image craftableImage;
        [SerializeField] private Image pinIconImage;

        protected override void OnSetData()
        {
            var data = Data as CraftableRecipeItemData;
            
            itemImage.sprite = data.Item.Image;
            highlightImage.gameObject.SetActive(false);
            craftableImage.gameObject.SetActive(data.IsCraftable());
            pinIconImage.gameObject.SetActive(false);
            placeholderImage.gameObject.SetActive(false);
            UpdateBackgroundColor();
        }

        protected override void OnSetSelected()
        {
            highlightImage.gameObject.SetActive(IsSelected);
        }

        protected override void OnSetBlanked()
        {
            base.OnSetBlanked();
            var data = Data as CraftableRecipeItemData;
            if (IsBlanked)
            {
                itemImage.gameObject.SetActive(false);
                craftableImage.gameObject.SetActive(false);
                pinIconImage.gameObject.SetActive(false);
                highlightImage.gameObject.SetActive(false);
                placeholderImage.gameObject.SetActive(true);
            }
            else
            {
                itemImage.gameObject.SetActive(true);
                craftableImage.gameObject.SetActive(data.IsCraftable());
                pinIconImage.gameObject.SetActive(IsPinned);
                highlightImage.gameObject.SetActive(IsSelected);
                placeholderImage.gameObject.SetActive(false);
            }
            UpdateBackgroundColor();
        }

        protected override void OnSetPinned()
        {
            base.OnSetPinned();
            pinIconImage.gameObject.SetActive(IsPinned);
        }

        private void UpdateBackgroundColor()
        {
            var color = backgroundImage.color;
            color.a = IsBlanked 
                ? 0.5f 
                : (Data as CraftableRecipeItemData).IsCraftable() 
                    ? 1f : 0.5f;
            backgroundImage.color = color;
        }
    }
}