using DG.Tweening;
using UnityEngine;

namespace Game.UI.View.Components.Transitions
{
    public abstract class TransitionAnimation : MonoBehaviour, ITransitionAnimation
    {
        [SerializeField] protected TransitionAnimationType transitionAnimationType;
        [SerializeField] protected float positionInTime;
        [SerializeField] protected float duration;
        [SerializeField] protected Ease ease;

        float ITransitionAnimation.Position => positionInTime;
        float ITransitionAnimation.Duration => duration;

        TransitionAnimationType ITransitionAnimation.TransitionType => transitionAnimationType;

        protected abstract Tween OnPlay();
        protected abstract void OnInitialize();
        protected abstract void OnReset();

        Tween ITransitionAnimation.Play()
        {
            return OnPlay();
        }

        void ITransitionAnimation.Initialize()
        {
            OnInitialize();
        }

        void ITransitionAnimation.Reset()
        {
            OnReset();
        }
    }
}