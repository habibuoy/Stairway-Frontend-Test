using Game.UI.View.Components;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class TabButton : BasicClickable
    {
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected string path;

        public bool IsActive { get; private set; }

        public string Path => path;

        public void Initialize()
        {
            nameText.text = path;
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public virtual void ToggleActive(bool active)
        {
            IsActive = active;
            OnToggleActive();
        }

        public virtual void OnToggleActive()
        {
            Color color = buttonImage.color;
            color.a = IsActive 
                ? 1f
                : IsHovered 
                    ? 0.25f
                    : 0f;

            buttonImage.color = color;
        }

        protected override void OnHoverBegun()
        {
            if (IsActive) return;
            base.OnHoverBegun();
        }

        protected override void OnHoverEnded()
        {
            IsHovered = false;
            if (IsActive) return;
            Color color = buttonImage.color;
            color.a =  0f;

            buttonImage.color = color;
        }
    }
}