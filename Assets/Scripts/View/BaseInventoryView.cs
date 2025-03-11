using System.Threading.Tasks;
using Game.UI.View.Components.Transitions;
using UnityEngine;

namespace Game.UI.View.Inventory
{
    public abstract class BaseInventoryView : MonoBehaviour
    {
        [SerializeField] private string path;

        protected Canvas canvas;
        protected ITransition transition;

        public virtual string Path => path;

        public void Initialize() 
        {
            canvas = GetComponent<Canvas>();
            if (TryGetComponent(out transition))
            {
                transition.Initialize();
            }
            OnInitialize();
            HideCanvas();
        }

        public Task Show()
        {
            return OnShow();
        }

        public Task Hide()
        {
            return OnHide();
        }

        protected void ShowCanvas()
        {
            canvas.enabled = true;
        }

        protected void HideCanvas()
        {
            canvas.enabled = false;
        }

        public virtual void OnInitialize() { }

        public virtual async Task OnShow()
        {
            ShowCanvas();
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
            HideCanvas();
        }
    }
}