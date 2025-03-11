using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View
{
    public class TabCycleButton : Clickable
    {
        [SerializeField] private Button button;
        [SerializeField] private KeyCode key;

        public event Action<TabCycleButton> KeyPressed;

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            KeyPressed = null;
        }

        protected override void OnClicked(ClickData clickData)
        {
            base.OnClicked(clickData);
            button.onClick?.Invoke();
        }

        private void Update()
        {
            if (key != KeyCode.None)
            {
                if (Input.GetKeyUp(key))
                {
                    KeyPressed?.Invoke(this);
                }
            }
        }
    }
}