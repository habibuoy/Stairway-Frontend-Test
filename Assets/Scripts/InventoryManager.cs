using System;
using System.Collections.Generic;
using System.Linq;
using Game.UI.Model;
using Game.UI.Model.Inventory;
using Game.UI.Presenter.Inventory;
using Game.UI.SO;
using Game.UI.View.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.UI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private ItemCollectionsSO itemCollections;
        [SerializeField] private GeneralInventoryView generalInventoryView;
        [SerializeField] private CraftingInventoryView craftingInventoryView;
        [SerializeField] private int generateItemCount = 50;
        [SerializeField] private int generateCraftableCount = 25;

        private const int MaxHealth = 420;
        private const int MaxEnergy = 400;
        private const int Gold = 840;

        private Canvas canvas;
        
        private GeneralInventoryPresenter generalInventoryPresenter;
        private CraftingInventoryPresenter craftingInventoryPresenter;

        public event Action Hidden;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;

            var characterModel = new CharacterModel(MaxHealth, MaxHealth, MaxEnergy, MaxEnergy);

            List<Item> items = GenerateItems();

            var sharedInventoryData = new SharedInventoryData(items.ToArray());
            var generalInventoryModel = new GeneralInventoryModel(sharedInventoryData, 
                characterModel, Gold);

            generalInventoryPresenter = new GeneralInventoryPresenter(generalInventoryModel, generalInventoryView);
            generalInventoryPresenter.Initialize();

            var craftingInventoryModel = new CraftingInventoryModel(sharedInventoryData);
            craftingInventoryPresenter = new CraftingInventoryPresenter(craftingInventoryModel, craftingInventoryView);
            craftingInventoryPresenter.Initialize();
            craftingInventoryPresenter.Hidden += OnCraftingHidden;
        }

        private void Start()
        {
            GameManager.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.CurrentTimeChanged -= OnCurrentTimeChanged;
            craftingInventoryPresenter.Hidden -= OnCraftingHidden;
        }

        public void Show()
        {
            canvas.enabled = true;
            craftingInventoryPresenter.Show();
        }

        public void Hide()
        {
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

        private void OnCurrentTimeChanged(DateTime dateTime)
        {
            generalInventoryPresenter.UpdateCurrentTime(dateTime, GameManager.Instance.FirstPlayDateTime);
        }

        private void OnCraftingHidden()
        {
            Hide();
        }
    }
}

