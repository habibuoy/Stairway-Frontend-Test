using TMPro;
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
            var color = backgroundImage.color;
            color.a = data.IsCraftable() ? 1f : 0.5f;
            backgroundImage.color = color;
        }

        protected override void OnSetSelected()
        {
            highlightImage.gameObject.SetActive(IsSelected);
        }

        protected override void OnSetBlanked()
        {
            base.OnSetBlanked();

            if (IsBlanked)
            {
                itemImage.gameObject.SetActive(false);
                craftableImage.gameObject.SetActive(false);
                pinIconImage.gameObject.SetActive(false);
                highlightImage.gameObject.SetActive(false);
                var color = backgroundImage.color;
                color.a = 0.5f;
                backgroundImage.color = color;
                placeholderImage.gameObject.SetActive(true);
            }
        }
    }
}