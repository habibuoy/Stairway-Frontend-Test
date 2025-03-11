using System;
using Game.UI.View;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TabView : MonoBehaviour
    {
        [SerializeField] private string defaultPath;
        [SerializeField] TabButton[] buttons;
        [SerializeField] private TabCycleButton cycleNextButton;
        [SerializeField] private TabCycleButton cyclePreviousButton;

        private int currentTabIndex;
        private CanvasGroup canvasGroup;
        
        public event Action<string> TabChanged;

        public void Initialize()
        {
            if (buttons == null
                || buttons.Length == 0)
            {
                Debug.LogError($"{nameof(TabView)}: Tab buttons are not assigned");
                return;
            }

            canvasGroup = GetComponent<CanvasGroup>();

            if (cyclePreviousButton)
            {
                cyclePreviousButton.Clicked += (clickable, clickData) => CyclePrevious();
                cyclePreviousButton.KeyPressed += (key) => CyclePrevious();
            }

            if (cycleNextButton)
            {
                cycleNextButton.Clicked += (clickable, clickData) => CycleNext();
                cycleNextButton.KeyPressed += (key) => CycleNext();
            }

            foreach (var button in buttons)
            {
                button.Initialize();
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                button.Clicked += OnButtonClicked;
                // if (button.Path == defaultPath)
                // {
                //     button.ToggleActive(true);
                //     currentTabIndex = i;
                //     continue;
                // }
                // button.ToggleActive(false);
            }
        }

        private void OnDestroy()
        {
            foreach (var button in buttons)
            {
                button.Clicked -= OnButtonClicked;
            }
            TabChanged = null;
        }

        private void SetActiveTab(string path)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                
                if (button.Path == path)
                {
                    button.ToggleActive(true);
                    currentTabIndex = i;
                    TabChanged?.Invoke(path);
                    continue;
                }
                button.ToggleActive(false);
            }
        }

        private void CycleNext()
        {
            currentTabIndex = (currentTabIndex + 1) % buttons.Length;
            if (buttons[currentTabIndex] is TabButton button)
            {
                SetActiveTab(button.Path);
            }
        }

        private void CyclePrevious()
        {
            currentTabIndex = currentTabIndex - 1 < 0 ? buttons.Length - 1 : currentTabIndex - 1;
            if (buttons[currentTabIndex] is TabButton button)
            {
                SetActiveTab(button.Path);
            }
        }

        private void OnButtonClicked(Clickable clickable, ClickData clickData)
        {
            var tabButton = clickable as TabButton;
            SetActiveTab(tabButton.Path);
        }

        public void SetInteractable(bool isInteractable = true)
        {
            if (!canvasGroup) return;
            canvasGroup.interactable = isInteractable;
            canvasGroup.blocksRaycasts = isInteractable;
        }

        public void SetTab(string path)
        {
            SetActiveTab(path);
        }
    }
}