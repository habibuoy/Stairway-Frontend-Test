using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.View
{
    public class Clickable : MonoBehaviour, IPointerClickHandler
    {
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

        protected virtual void OnAwake() { }
        protected virtual void OnDestroyed() { }

        protected virtual void OnClicked(ClickData clickData) { }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                default:
                    Clicked?.Invoke(this, ClickData.Left());
                    break;
                case PointerEventData.InputButton.Right:
                    Clicked?.Invoke(this, ClickData.Right());
                    break;
                case PointerEventData.InputButton.Middle:
                    Clicked?.Invoke(this, ClickData.Middle());
                    break;
            }
        }

        private void Click(ClickData clickData)
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