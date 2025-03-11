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
        private readonly List<ListViewItem> sortedAndPinnedListViewItems = new();

        private Func<ListViewItem, bool> sorterInclusionFunc;
        private Action<ListViewItem> sorterIncludedAction;
        private Action<ListViewItem> sorterExcludedAction;

        public bool IsItemSelectable { get; private set; }
        public bool IsItemHoverable { get; private set; }
        public RectTransform Content => scrollRect.content;
        public event Action<ListViewItem, ClickData> ItemClicked;
        public event Action<ListViewItem> ItemBegunHover;
        public event Action<ListViewItem> ItemEndedHover;
        public event Action<ListViewItem, ClickData> ItemBegunClick;
        public event Action<ListViewItem, ClickData> ItemEndedClick;

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
            bool existingItems = sortedAndPinnedListViewItems.Count > 0;
            
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
                listItem.ClickBegun += OnItemBegunClick;
                listItem.ClickEnded += OnItemEndedClick;
                listItem.SetInteractable();
                listItem.SetData(data, i);
                if (existingItems
                    && sortedAndPinnedListViewItems.Find(item => item.Data.Id == data.Id) is ListViewItem existing)
                {
                    if (existing.IsPinned)
                    {
                        listItem.SetPinned();
                    }
                }
                customConfiguration?.Invoke(listItem);
                listViewItems.Add(listItem);
            }

            if (existingItems)
            {
                if (!UpdateSortedList())
                {
                    sortedAndPinnedListViewItems.Clear();
                    sortedAndPinnedListViewItems.AddRange(listViewItems);
                    sortedAndPinnedListViewItems.Sort();
                }

                RearrangeItemPositions();
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

        public void SortItems(Func<ListViewItem, bool> include,
            Action<ListViewItem> callbackForIncluded, 
            Action<ListViewItem> callbackForExcluded)
        {
            sortedAndPinnedListViewItems.Clear();

            sorterInclusionFunc = include;
            sorterIncludedAction = callbackForIncluded;
            sorterExcludedAction = callbackForExcluded;

            UpdateSortedList();
            RearrangeItemPositions();
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
                listItem.HoverBegun -= OnItemBegunHover;
                listItem.HoverEnded -= OnItemEndedHover;
                listItem.ClickBegun -= OnItemBegunClick;
                listItem.ClickEnded -= OnItemEndedClick;
            }
        }

        private void OnItemClicked(Clickable clickable, ClickData clickData)
        {
            if (!IsItemSelectable) return;
            ItemClicked?.Invoke(clickable as ListViewItem, clickData);
        }

        private void OnItemBegunHover(ListViewItem listViewItem)
        {
            ItemBegunHover?.Invoke(listViewItem);
        }

        private void OnItemEndedHover(ListViewItem listViewItem)
        {
            ItemEndedHover?.Invoke(listViewItem);
        }

        private void OnItemBegunClick(Clickable clickable, ClickData clickData)
        {
            ItemBegunClick?.Invoke(clickable as ListViewItem, clickData);
        }

        private void OnItemEndedClick(Clickable clickable, ClickData clickData)
        {
            ItemEndedClick?.Invoke(clickable as ListViewItem, clickData);
        }

        public void SelectItem(int itemIndex)
        {
            foreach (var listItem in listViewItems)
            {
                if (listItem.Index == itemIndex
                    && listItem.IsInteractable)
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

        public void SetPinned(int itemIndex, bool pinned = true)
        {
            foreach (var listItem in listViewItems)
            {
                if (listItem.Index == itemIndex)
                {
                    listItem.SetPinned(pinned);
                }
            }

            if (!UpdateSortedList())
            {
                sortedAndPinnedListViewItems.Clear();
                sortedAndPinnedListViewItems.AddRange(listViewItems);
                sortedAndPinnedListViewItems.Sort();
            }

            RearrangeItemPositions();
        }

        private bool UpdateSortedList()
        {
            if (sorterInclusionFunc == null) return false;

            foreach (var listItem in listViewItems)
            {
                if (sorterInclusionFunc.Invoke(listItem) || listItem.IsPinned)
                {
                    sorterIncludedAction?.Invoke(listItem);
                    sortedAndPinnedListViewItems.Add(listItem);
                }
                else
                {
                    sorterExcludedAction?.Invoke(listItem);
                }
            }

            sortedAndPinnedListViewItems.Sort();

            foreach (var listItem in listViewItems)
            {
                if (!sortedAndPinnedListViewItems.Contains(listItem))
                {
                    sortedAndPinnedListViewItems.Add(listItem);
                }
            }

            return true;
        }

        private void RearrangeItemPositions()
        {
            for (int i = 0; i < sortedAndPinnedListViewItems.Count; i++)
            {
                ArrangePositionAndIndex(sortedAndPinnedListViewItems[i], i);
            }
        }

        private void ArrangePositionAndIndex(ListViewItem item, int index)
        {
            if (item == null) return;
            
            item.SetIndex(index);
            item.transform.SetSiblingIndex(index);
        }

        public ListViewItem GetListViewItem(Predicate<ListViewItem> predicate)
        {
            return listViewItems.Find(predicate);
        }
    }
}