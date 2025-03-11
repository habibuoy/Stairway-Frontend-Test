using System;
using Game.UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private Canvas canvas; 
        [SerializeField] private Button openInventoryButton;
        [SerializeField] private Toggle animationToggle;

        private const int HoursMultiplier = 24;
        private const float TimeUpdateRate = 1f;

        private static GameManager instance;

        private float timeUpdateTimer;

        public static GameManager Instance => instance;
        public DateTime FirstPlayDateTime { get; private set; }
        public DateTime CurrentTime { get; private set; }
        public bool UseAnimation => animationToggle.isOn;

        public event Action<DateTime> CurrentTimeChanged;

        private void Awake()
        {
            instance = this;
            if (instance != this)
            {
                Destroy(gameObject);
            }

            FirstPlayDateTime = DateTime.Now;
            CurrentTime = FirstPlayDateTime;

            ResetTimeUpdateTimer();

            openInventoryButton.onClick.AddListener(OpenInventory);
            inventoryManager.Hidden += OnInventoryHidden;
        }

        private void OnDestroy()
        {
            CurrentTimeChanged = null;
            openInventoryButton.onClick.RemoveListener(OpenInventory);
            inventoryManager.Hidden -= OnInventoryHidden;
        }

        private void Update()
        {
            if (timeUpdateTimer > 0f)
            {
                timeUpdateTimer -= Time.deltaTime;
                if (timeUpdateTimer <= 0)
                {
                    CurrentTime = CurrentTime.AddHours(HoursMultiplier);
                    CurrentTimeChanged?.Invoke(CurrentTime);
                    ResetTimeUpdateTimer();
                }
            }
        }

        private void ResetTimeUpdateTimer()
        {
            timeUpdateTimer = TimeUpdateRate;
        }

        private void OpenInventory()
        {
            canvas.enabled = false;
            _ = inventoryManager.Show();
        }

        private void OnInventoryHidden()
        {
            canvas.enabled = true;
        }
    }
}