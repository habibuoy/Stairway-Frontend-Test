using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.View
{
    public class Clickable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsInteractable { get; private set; } = true;
        public bool IsClicking { get; private set; }

        public event Action<Clickable, ClickData> Clicked;
        public event Action<Clickable, ClickData> ClickBegun;
        public event Action<Clickable, ClickData> ClickEnded;

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

        protected virtual void OnClicked(ClickData clickData) { }
        protected virtual void OnClickBegun(ClickData clickData) { }
        protected virtual void OnClickEnded(ClickData clickData) { }

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

        protected void BeginClick(ClickData clickData)
        {
            IsClicking = true;
            OnClickBegun(clickData);
            ClickBegun?.Invoke(this, clickData);
        }

        protected void EndClick(ClickData clickData)
        {
            IsClicking = false;
            OnClickEnded(clickData);
            ClickEnded?.Invoke(this, clickData);
        }

        protected void Click(ClickData clickData)
        {
            OnClicked(clickData);
            Clicked?.Invoke(this, clickData);
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