using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.View
{
    public class Clickable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        public bool IsInteractable { get; private set; } = true;
        public bool IsClicking { get; private set; }
        public bool IsHovered { get; protected set; }

        public event Action<Clickable, ClickData> Clicked;
        public event Action<Clickable, ClickData> ClickBegun;
        public event Action<Clickable, ClickData> ClickEnded;
        public event Action<Clickable> HoverBegun;
        public event Action<Clickable> HoverEnded;

        private void Awake()
        {
            OnAwake();
        }

        private void OnDestroy()
        {
            Clicked = null;
            ClickBegun = null;
            ClickEnded = null;
            OnDestroyed();
        }

        public void SetInteractable(bool interactable = true)
        {
            IsInteractable = interactable;
        }

        protected virtual void OnAwake() { }

        protected virtual void OnDestroyed() { }

        protected virtual void OnClicked(ClickData clickData) 
        {
            Clicked?.Invoke(this, clickData);
        }

        protected virtual void OnClickBegun(ClickData clickData) 
        {
            ClickBegun?.Invoke(this, clickData);
        }

        protected virtual void OnClickEnded(ClickData clickData) 
        {
            ClickEnded?.Invoke(this, clickData);
        }

        protected virtual void OnHoverBegun() 
        {
            IsHovered = true;
        }
        
        protected virtual void OnHoverEnded() 
        {
            IsHovered = false;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable) return;
            switch (eventData.button)
            {
                default:
                    Click(ClickData.Left());
                    break;
                case PointerEventData.InputButton.Right:
                    Click(ClickData.Right());
                    break;
                case PointerEventData.InputButton.Middle:
                    Click(ClickData.Middle());
                    break;
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!IsInteractable) return;
            switch (eventData.button)
            {
                default:
                    BeginClick(ClickData.Left());
                    break;
                case PointerEventData.InputButton.Right:
                    BeginClick(ClickData.Right());
                    break;
                case PointerEventData.InputButton.Middle:
                    BeginClick(ClickData.Middle());
                    break;
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (!IsInteractable || !IsClicking) return;
            switch (eventData.button)
            {
                default:
                    EndClick(ClickData.Left());
                    break;
                case PointerEventData.InputButton.Right:
                    EndClick(ClickData.Right());
                    break;
                case PointerEventData.InputButton.Middle:
                    EndClick(ClickData.Middle());
                    break;
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!IsInteractable) return;
            BeginHover();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!IsInteractable || !IsHovered) return;
            EndHover();
        }

        protected void BeginClick(ClickData clickData)
        {
            IsClicking = true;
            OnClickBegun(clickData);
        }

        protected void EndClick(ClickData clickData)
        {
            IsClicking = false;
            OnClickEnded(clickData);
        }

        protected void Click(ClickData clickData)
        {
            OnClicked(clickData);
        }

        protected void BeginHover()
        {
            OnHoverBegun();
            HoverBegun?.Invoke(this);
        }

        protected void EndHover()
        {
            OnHoverEnded();
            HoverEnded?.Invoke(this);
        }
    }

    public struct ClickData
    {
        public PointerEventData.InputButton Button { get; }

        public ClickData(PointerEventData.InputButton button)
        {
            Button = button;
        }

        public static ClickData Left()
        {
            return new ClickData(PointerEventData.InputButton.Left);
        }

        public static ClickData Right()
        {
            return new ClickData(PointerEventData.InputButton.Right);
        }

        public static ClickData Middle()
        {
            return new ClickData(PointerEventData.InputButton.Middle);
        }
    }
}