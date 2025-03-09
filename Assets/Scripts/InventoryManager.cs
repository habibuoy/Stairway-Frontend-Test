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
        [SerializeField] private int generateitemCount = 50;
        [SerializeField] private int generateCraftableCount = 25;

        private const int MaxHealth = 420;
        private const int MaxEnergy = 400;
        private const int Gold = 840;
        
        private GeneralInventoryPresenter generalInventoryPresenter;

        private void Awake()
        {
            var characterModel = new CharacterModel(MaxHealth, MaxHealth, MaxEnergy, MaxEnergy);

            List<Item> items = GenerateItems();

            var sharedInventoryData = new SharedInventoryData(items.ToArray());
            var generalInventoryModel = new GeneralInventoryModel(sharedInventoryData, 
                characterModel, Gold);

            generalInventoryPresenter = new GeneralInventoryPresenter(generalInventoryModel, generalInventoryView);
            generalInventoryPresenter.Initialize();

            var craftingInventoryModel = new CraftingInventoryModel(sharedInventoryData);
            var craftingInventoryPresenter = new CraftingInventoryPresenter(craftingInventoryModel, craftingInventoryView);
            craftingInventoryPresenter.Initialize();
        }

        private void Start()
        {
            GameManager.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.CurrentTimeChanged -= OnCurrentTimeChanged;
        }

        private List<Item> GenerateItems()
        {
            List<Item> items = new();
            int itemCount = itemCollections.Items.Count;
            int craftableCount = itemCollections.Craftables.Count;

            for (int i = 0; i < generateitemCount; i++)
            {
                var so = itemCollections.Items.ElementAt(Random.Range(0, itemCount));
                var item = new Item(so, Random.Range(1, 10));
                items.Add(item);
            }

            for (int i = 0; i < generateCraftableCount; i++)
            {
                var so = itemCollections.Craftables.ElementAt(Random.Range(0, craftableCount));
                var item = new Item(so, Random.Range(1, 10));
                items.Add(item);
            }

            return items;
        }

        private void OnCurrentTimeChanged(DateTime dateTime)
        {
            generalInventoryPresenter.UpdateCurrentTime(dateTime, GameManager.Instance.FirstPlayDateTime);
        }
    }
}

