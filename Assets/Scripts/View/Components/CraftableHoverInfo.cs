using DG.Tweening;
using Game.Extensions;
using TMPro;
using UnityEngine;

namespace Game.UI.View.Components
{
    public class CraftableHoverInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hoverInfoNameText;
        [SerializeField] private TextMeshProUGUI hoverOwnedCountText;
        [SerializeField] private TextMeshProUGUI hoverCraftabilityCountText;
        [SerializeField] private Vector2 showPositionOffset = new Vector2(0, -62f);
        [SerializeField] private RectTransform contentParent;
        [SerializeField] private Vector2 showAnimationOffset = new Vector2(0, 16f);
        [SerializeField] private float showAnimationDuration = 0.1f;
    
        private Vector2 contentParentDefaultPosition;
        private CanvasGroup canvasGroup;
        private Sequence showSequence;

        public void Initialize()
        {
            contentParentDefaultPosition = contentParent.anchoredPosition;
            canvasGroup = contentParent.GetComponent<CanvasGroup>();
        }

        public void ShowInfo(CraftableRecipeItemData itemData, Vector2 position)
        {
            hoverInfoNameText.text = itemData.Item.ItemName;
            hoverOwnedCountText.text = itemData.Item.Count.ToString();
            int craftabilityCount = itemData.GetCraftabilityCount();
            string color = craftabilityCount > 0 
                ? ColorExtensions.GetSufficientCraftStringColor()
                : ColorExtensions.GetInsufficientCraftStringColor();
            hoverCraftabilityCountText.text = $"<color={color}>{craftabilityCount}</color>";

            (transform as RectTransform).anchoredPosition = position + showPositionOffset;

            showSequence?.Complete();
            contentParent.anchoredPosition = contentParentDefaultPosition + showAnimationOffset;
            canvasGroup.alpha = 0f;

            gameObject.SetActive(true);
            showSequence = DOTween.Sequence()
                .Append(contentParent.DOAnchorPos(contentParentDefaultPosition, showAnimationDuration).SetEase(Ease.OutBack))
                .Join(canvasGroup.DOFade(1f, showAnimationDuration))
                .OnComplete(() => contentParent.anchoredPosition = contentParentDefaultPosition)
                .Play();
        }

        public void HideInfo()
        {
            showSequence?.Complete();
            gameObject.SetActive(false);
        }
    }
}