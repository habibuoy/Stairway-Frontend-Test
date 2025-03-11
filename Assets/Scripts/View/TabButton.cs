using System;
using Game.UI.View;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class TabButton : Clickable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image buttonImage;
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected string path;

        public bool IsActive { get; private set; }
        public bool IsHovered { get; private set; }

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

        public virtual void OnBegunHover()
        {
            if (IsActive) return;
            
            Color color = buttonImage.color;
            color.a = 0.25f;

            buttonImage.color = color;
        }

        public virtual void OnEndedHover() 
        {
            Color color = buttonImage.color;
            color.a = 0f;

            buttonImage.color = color;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            OnBegunHover();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            OnEndedHover();
        }
    }
}