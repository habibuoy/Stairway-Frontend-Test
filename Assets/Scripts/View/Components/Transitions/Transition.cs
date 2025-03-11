using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.UI.View.Components.Transitions
{
    public class Transition : MonoBehaviour, ITransition
    {
        private ITransitionAnimation[] animations;

        private Sequence transitionSequence;

        void ITransition.Initialize()
        {
            animations = GetComponentsInChildren<ITransitionAnimation>(true);
            foreach (var animation in animations)
            {
                animation.Initialize();
            }
        }

        Task ITransition.In()
        {
            transitionSequence?.Complete();
            transitionSequence = DOTween.Sequence();
            foreach (var animation in animations)
            {
                if (animation.TransitionType != TransitionAnimationType.In) continue;
                transitionSequence.Insert(animation.Position, animation.Play());
            }
            return transitionSequence.Play().AsyncWaitForCompletion();
        }

        Task ITransition.Out()
        {
            transitionSequence?.Complete();
            transitionSequence = DOTween.Sequence();
            foreach (var animation in animations)
            {
                if (animation.TransitionType != TransitionAnimationType.Out) continue;
                transitionSequence.Insert(animation.Position, animation.Play());
            }
            return transitionSequence.Play().OnComplete(() => 
            {
                foreach (var animation in animations)
                {
                    animation.Reset();
                }
            }).AsyncWaitForCompletion();
        }
    }
}