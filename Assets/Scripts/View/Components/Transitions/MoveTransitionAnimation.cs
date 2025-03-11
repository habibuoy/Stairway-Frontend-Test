using DG.Tweening;
using UnityEngine;

namespace Game.UI.View.Components.Transitions
{
    public class MoveTransitionAnimation : TransitionAnimation
    {
        [SerializeField] private Vector2 offsetPosition;

        private RectTransform rectTransform;
        private Vector2 defaultPosition;

        protected override void OnInitialize()
        {
            rectTransform = transform as RectTransform;
            defaultPosition = rectTransform.anchoredPosition;
        }

        protected override Tween OnPlay()
        {
            if (transitionAnimationType == TransitionAnimationType.In)
            {
                rectTransform.anchoredPosition = defaultPosition + offsetPosition;
                return rectTransform.DOAnchorPos(defaultPosition, duration).SetEase(ease);
            }

            rectTransform.anchoredPosition = defaultPosition;
                return rectTransform.DOAnchorPos(defaultPosition + offsetPosition, duration).SetEase(ease);
        }

        protected override void OnReset()
        {
            rectTransform.anchoredPosition = defaultPosition;
        }
    }
}