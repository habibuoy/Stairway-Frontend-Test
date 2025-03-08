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

        private void Awake()
        {
            if (buttons == null
                || buttons.Length == 0)
            {
                Debug.LogError($"{nameof(TabView)}: Tab buttons first are not assigned");
                return;
            }

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

            if (string.IsNullOrEmpty(defaultPath))
            {
                buttons[0].ToggleActive(true);
            }
            else
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    var button = buttons[i];
                    button.Clicked += OnButtonClicked;
                    if (button.Path == defaultPath)
                    {
                        button.ToggleActive(true);
                        currentTabIndex = i;
                        continue;
                    }
                    button.ToggleActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var button in buttons)
            {
                button.Clicked -= OnButtonClicked;
            }
        }

        private void SetActiveTab(string path)
        {
            foreach (var button in buttons)
            {
                if (button.Path == path)
                {
                    button.ToggleActive(true);
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
            foreach (var button in buttons)
            {
                if (button.Path == tabButton.Path)
                {
                    button.ToggleActive(true);
                    continue;
                }
                button.ToggleActive(false);
            }
        }
    }
}