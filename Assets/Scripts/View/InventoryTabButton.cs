using DG.Tweening;
using UnityEngine;

namespace Game.UI
{
    public class InventoryTabButton : TabButton
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private Vector3 hoverAnimationScale = new Vector3(1.2f , 1.2f, 1.2f);
        [SerializeField] private float hoverAnimationDuration = 0.1f;

        private Tween hoverTween;
        private Vector3 defaultScale;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            defaultScale = buttonImage.transform.localScale;
        }

        public override void OnToggleActive()
        {
            buttonImage.color = IsActive ? activeColor : inactiveColor;
            nameText.gameObject.SetActive(IsActive);
            hoverTween?.Complete();
        }

        protected override void OnHoverBegun()
        {
            base.OnHoverBegun();

            if (IsActive) return;
            hoverTween?.Complete();

            hoverTween = buttonImage.transform.DOScale(hoverAnimationScale, hoverAnimationDuration)
                .SetEase(Ease.OutBack)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => transform.localScale = defaultScale)
                .Play();
        }

        protected override void OnHoverEnded()
        {
            buttonImage.color = buttonImage.color = IsActive ? activeColor : inactiveColor;
            hoverTween?.Complete();
        }
    }
}