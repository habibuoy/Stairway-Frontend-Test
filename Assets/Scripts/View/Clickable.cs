using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.View
{
    public class Clickable : MonoBehaviour, IPointerClickHandler
    {
        public bool Interactable { get; private set; } = true;
        public event Action<Clickable, ClickData> Clicked;

        private void Awake()
        {
            OnAwake();
        }

        private void OnDestroy()
        {
            Clicked = null;
            OnDestroyed();
        }

        public void SetInteractable(bool interactable = true)
        {
            Interactable = interactable;
        }

        protected virtual void OnAwake() { }
        protected virtual void OnDestroyed() { }

        protected virtual void OnClicked(ClickData clickData) { }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable) return;
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