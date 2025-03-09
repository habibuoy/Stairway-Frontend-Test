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
        public RectTransform Content => scrollRect.content;
        public event Action<ListViewItem> ItemClicked;

        public void UpdateList(List<IListData> listData)
        {
            int dataCount = listData.Count;

            if (listData == null
                || dataCount == 0
                || Content == null
                || listItemPrefab == null
                || !listItemPrefab.TryGetComponent<ListViewItem>(out var component))
            {
                Debug.LogError($"{nameof(ListView)}: Error while initializing list");
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
                listItem.SetData(data, i);
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

        public void ToggleItemSelectable(bool selectable)
        {
            IsItemSelectable = selectable;
        }
    }
}