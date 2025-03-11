using DG.Tweening;
using UnityEngine;

namespace Game.UI.View.Components.Transitions
{
    public class FadeTransitionAnimation : TransitionAnimation
    {
        private CanvasGroup canvasGroup;
        private float defaultAlpha;

        protected override void OnInitialize()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup)
            {
                defaultAlpha = canvasGroup.alpha;
            }
        }

        protected override Tween OnPlay()
        {
            if (canvasGroup == null) return null;

            if (transitionAnimationType == TransitionAnimationType.In)
            {
                canvasGroup.alpha = 0f;
                return canvasGroup.DOFade(defaultAlpha, duration).SetEase(ease);
            }
            
            canvasGroup.alpha = 1f;
            return canvasGroup.DOFade(0f, duration).SetEase(ease);
        }

        protected override void OnReset()
        {
            canvasGroup.alpha = defaultAlpha;
        }
    }
}