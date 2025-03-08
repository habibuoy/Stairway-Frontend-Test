using System;
using System.Collections.Generic;
using Game.UI.Model;
using Game.UI.Model.Inventory;
using Game.UI.Presenter.Inventory;
using Game.UI.View.Inventory;
using UnityEngine;

namespace Game.UI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GeneralInventoryView generalInventoryView;

        private const int MaxHealth = 420;
        private const int MaxEnergy = 400;
        private const int Gold = 840;
        
        private GeneralInventoryPresenter generalInventoryPresenter;

        private void Awake()
        {
            var characterModel = new CharacterModel(MaxHealth, MaxHealth, MaxEnergy, MaxEnergy);

            List<Item> items = new();
            var sharedInventoryData = new SharedInventoryData(items.ToArray());
            var generalInventoryModel = new GeneralInventoryModel(sharedInventoryData, 
                characterModel, Gold);

            generalInventoryPresenter = new GeneralInventoryPresenter(generalInventoryModel, generalInventoryView);
            generalInventoryPresenter.Initialize();

            GameManager.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.CurrentTimeChanged -= OnCurrentTimeChanged;
        }

        private void OnCurrentTimeChanged(DateTime dateTime)
        {
            generalInventoryPresenter.UpdateCurrentTime(dateTime, GameManager.Instance.FirstPlayDateTime);
        }
    }
}

