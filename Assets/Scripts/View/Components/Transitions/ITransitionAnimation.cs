using System.Threading.Tasks;
using DG.Tweening;

namespace Game.UI.View.Components.Transitions
{
    public interface ITransitionAnimation
    {
        TransitionAnimationType TransitionType { get; }

        float Position { get; }

        float Duration { get; }

        Tween Play();

        void Initialize();
        
        void Reset();

    }

    public enum TransitionAnimationType
    {
        None,
        In,
        Out,
    }
}