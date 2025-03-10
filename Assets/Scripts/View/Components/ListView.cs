using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Components
{
    [RequireComponent(typeof(ScrollRect))]
    public class ListView : MonoBehaviour
    {
        [SerializeField] private GameObject listItemPrefab;
        [SerializeField] private ScrollRect scrollRect;
        
        private readonly List<ListViewItem> listViewItems = new();

        public bool IsItemSelectable { get; private set; }
        public bool IsItemHoverable { get; private set; }
        public RectTransform Content => scrollRect.content;
        public event Action<ListViewItem> ItemClicked;
        public event Action<ListViewItem> ItemBegunHover;
        public event Action<ListViewItem> ItemEndedHover;

        public void UpdateList(List<IListData> listData, Action<ListViewItem> customConfiguration = null)
        {
            int dataCount = listData.Count;

            if (listData == null
                || dataCount == 0
                || Content == null
                || listItemPrefab == null
                || !listItemPrefab.TryGetComponent<ListViewItem>(out var component))
            {
                Debug.LogError($"{nameof(ListView)} ({name}): Error while updating list", gameObject);
                return;
            }

            ClearList();
            
            for (int i = 0; i < listData.Count; i++)
            {
                var data = listData[i];

                var listItemObj = Instantiate(listItemPrefab, Content);
                if (!listItemObj.TryGetComponent<ListViewItem>(out var listItem))
                {
                    Destroy(listItemObj);
                    continue;
                }

                listItem.Clicked += OnItemClicked;
                listItem.HoverBegun += OnItemBegunHover;
                listItem.HoverEnded += OnItemEndedHover;
                listItem.SetInteractable();
                listItem.SetData(data, i);
                customConfiguration?.Invoke(listItem);
                listViewItems.Add(listItem);
            }
        }

        public void ClearList()
        {
            if (Content == null)
            {
                Debug.LogError($"{nameof(ListView)}: Error while clearing list, parent is null");
                return;
            }

            UnsubscribeAllItems();
            listViewItems.Clear();

            for (int i = 0; i < Content.childCount; i++)
            {
                Destroy(Content.GetChild(i).gameObject);
            }
        }

        public void Destroy()
        {
            ItemClicked = null;
        }

        private void UnsubscribeAllItems()
        {
            foreach (var listItem in listViewItems)
            {
                listItem.Clicked -= OnItemClicked;
            }
        }

        private void OnItemClicked(Clickable clickable, ClickData clickData)
        {
            if (!IsItemSelectable) return;
            ItemClicked?.Invoke(clickable as ListViewItem);
        }

        private void OnItemBegunHover(ListViewItem listViewItem)
        {
            ItemBegunHover?.Invoke(listViewItem);
        }

        private void OnItemEndedHover(ListViewItem listViewItem)
        {
            ItemEndedHover?.Invoke(listViewItem);
        }

        public void SelectItem(int itemIndex)
        {
            foreach (var listItem in listViewItems)
            {
                if (listItem.Index == itemIndex)
                {
                    listItem.SelectItem();
                    return;
                }
            }
        }

        public void SetSelected(int itemIndex)
        {
            foreach (var listItem in listViewItems)
            {
                listItem.SetSelected(listItem.Index == itemIndex);
            }
        }

        public void SetHovered(int itemIndex, bool hovered = true)
        {
            foreach (var listItem in listViewItems)
            {
                listItem.SetHovered(listItem.Index == itemIndex && hovered);
            }
        }

        public void ToggleItemSelectable(bool selectable)
        {
            IsItemSelectable = selectable;
        }

        public ListViewItem GetListViewItem(Predicate<ListViewItem> predicate)
        {
            return listViewItems.Find(predicate);
        }
    }
}