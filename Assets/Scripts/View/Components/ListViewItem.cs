using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.View.Components
{
    public abstract class ListViewItem : Clickable, IPointerEnterHandler, IPointerExitHandler
    {
        private RectTransform rectTransform;

        public int Index { get; private set; }
        public IListData Data { get; private set; }
        public bool IsSelected { get; private set; }
        public bool IsHovered { get; private set; }
        public bool IsBlanked { get; private set; }
        public RectTransform RectTransform => rectTransform;

        public event Action<ListViewItem> HoverBegun;
        public event Action<ListViewItem> HoverEnded;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        public void SelectItem()
        {
            Click(ClickData.Left());
        }

        public void SetSelected(bool selected = true)
        {
            IsSelected = selected;
            OnSetSelected();
        }

        public void SetBlanked(bool blanked = true)
        {
            IsBlanked = blanked;
            OnSetBlanked();
        }

        public void SetHovered(bool hovered = true)
        {
            IsHovered = hovered;
            OnSetHovered();
        }

        public void SetData(IListData data, int index)
        {
            Data = data;
            SetIndex(index);
            if (data.IsBlankData()) return;
            OnSetData();
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        protected abstract void OnSetData();
        protected virtual void OnSetSelected() { }
        protected virtual void OnSetHovered() { }
        protected virtual void OnSetBlanked() { }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable) return;
            HoverBegun?.Invoke(this);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!Interactable) return;
            HoverEnded?.Invoke(this);
        }
    }
}