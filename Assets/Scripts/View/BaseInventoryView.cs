using System.Threading.Tasks;
using Game.UI.View.Components.Transitions;
using UnityEngine;

namespace Game.UI.View.Inventory
{
    public abstract class BaseInventoryView : MonoBehaviour
    {
        protected Canvas canvas;
        protected ITransition transition;

        public void Initialize() 
        {
            canvas = GetComponent<Canvas>();
            if (TryGetComponent(out transition))
            {
                transition.Initialize();
            }
            OnInitialize();
        }

        public Task Show()
        {
            return OnShow();
        }

        public Task Hide()
        {
            return OnHide();
        }

        public virtual void OnInitialize() { }

        public virtual async Task OnShow()
        {
            canvas.enabled = true;
            if (GameManager.Instance.UseAnimation)
            {
                if (transition != null)
                {
                    await transition.In();
                }
            }
        }
        
        public virtual async Task OnHide() 
        {
            if (GameManager.Instance.UseAnimation)
            {
                if (transition != null)
                {
                    await transition.Out();
                }
            }
            canvas.enabled = false;
        }
    }
}