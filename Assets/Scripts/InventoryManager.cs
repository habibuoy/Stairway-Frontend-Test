using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.UI.Model;
using Game.UI.Model.Inventory;
using Game.UI.Model.Inventory.factory;
using Game.UI.Presenter.Inventory;
using Game.UI.Presenter.Inventory.Factory;
using Game.UI.SO;
using Game.UI.View.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.UI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private ItemCollectionsSO itemCollections;
        [SerializeField] private BaseInventoryView[] inventoryViews;
        [SerializeField] private TabView tabView;
        [SerializeField] private int generateItemCount = 50;
        [SerializeField] private int generateCraftableCount = 25;

        private const int MaxHealth = 420;
        private const int MaxEnergy = 400;
        private const int Gold = 840;

        private Canvas canvas;
        private InventoryPresenterManager presenterManager;

        public event Action Hidden;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;

            var characterModel = new CharacterModel(MaxHealth, MaxHealth, MaxEnergy, MaxEnergy);

            List<Item> items = GenerateItems();

            var sharedInventoryData = new SharedInventoryData(items.ToArray());

            IInventoryModelFactory modelFactory = new InventoryModelFactory(characterModel, sharedInventoryData, Gold);
            IInventoryPresenterFactory presenterFactory = new InventoryPresenterFactory();
            presenterManager = new InventoryPresenterManager(modelFactory, presenterFactory, inventoryViews);
            presenterManager.CloseInputted += OnCloseInputted;

            tabView.Initialize();
            tabView.TabChanged += OnTabChanged;
        }

        private void Start()
        {
            GameManager.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.CurrentTimeChanged -= OnCurrentTimeChanged;
            tabView.TabChanged -= OnTabChanged;
            presenterManager.CloseInputted -= OnCloseInputted;
            presenterManager.Dispose();
        }

        public async Task Show()
        {
            canvas.enabled = true;
            await presenterManager.ShowParent();
            tabView.SetTab("Crafting");
        }

        public async Task Hide()
        {
            await presenterManager.HideInventory();
            canvas.enabled = false;
            Hidden?.Invoke();
        }

        private List<Item> GenerateItems()
        {
            List<Item> items = new();
            int itemCount = itemCollections.Items.Count;
            int craftableCount = itemCollections.Craftables.Count;

            generateItemCount = itemCount >= generateItemCount 
                ? itemCount : generateItemCount;

            for (int i = 0; i < generateItemCount; i++)
            {
                Item item = null;

                if (i < itemCount)
                {
                    var so = itemCollections.Items.ElementAt(i);
                    item = new Item(so, Random.Range(1, 5));
                }
                else
                {
                    item = Item.Blank();
                }
                items.Add(item);
            }

            generateCraftableCount = craftableCount >= generateCraftableCount 
                ? craftableCount : generateCraftableCount;

            for (int i = 0; i < generateCraftableCount; i++)
            {
                Item item = null;

                if (i < craftableCount)
                {
                    var so = itemCollections.Craftables.ElementAt(i);
                    item = new Item(so, Random.Range(1, 5));
                }
                else
                {
                    item = Item.Blank();
                }

                items.Add(item);
            }

            return items;
        }

        private void OnCloseInputted()
        {
            _ = Hide();
        }

        private void OnCurrentTimeChanged(DateTime dateTime)
        {
            presenterManager.UpdateCurrentTime(dateTime, GameManager.Instance.FirstPlayDateTime);
        }

        private void OnTabChanged(string path)
        {
            _ = ChangeTab(path);
        }

        private async Task ChangeTab(string path)
        {
            tabView.SetInteractable(false);
            await presenterManager.ShowPage(path);
            tabView.SetInteractable(true);
        }
    }
}

