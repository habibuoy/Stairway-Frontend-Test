using System;
using Game.UI.Model.Inventory;
using Game.UI.View;
using Game.UI.View.Inventory;
using UnityEngine.EventSystems;

namespace Game.UI.Presenter.Inventory
{
    public class GeneralInventoryPresenter : BaseInventoryPresenter<GeneralInventoryModel, GeneralInventoryView>
    {
        private const int IncreaseGoldAmount = 25;
        private const int DecreaseGoldAmount = 25;
        private const int IncreaseHealthAmount = 10;
        private const int DecreaseHealthAmount = 10;
        private const int IncreaseEnergyAmount = 10;
        private const int DecreaseEnergyAmount = 10;

        public GeneralInventoryPresenter(GeneralInventoryModel model, GeneralInventoryView view) 
            : base(model, view) { }

        protected override void OnInitialize() 
        {
            view.UpdateGold(model.GoldAmount);
            view.UpdateHealth(model.CharacterModel.Health, model.CharacterModel.MaxHealth);
            view.UpdateEnergy(model.CharacterModel.Energy, model.CharacterModel.MaxEnergy);

            model.CharacterModel.HealthChanged += OnHealthChanged;
            view.HealthClicked += OnHealthClicked;

            model.CharacterModel.EnergyChanged += OnEnergyChanged;
            view.EnergyClicked += OnEnergyClicked;

            model.GoldChanged += OnGoldChanged;
            view.GoldClicked += OnGoldClicked;
        }

        protected override void OnDestroy()
        {
            model.CharacterModel.HealthChanged -= OnHealthChanged;
            view.HealthClicked -= OnHealthClicked;

            model.CharacterModel.EnergyChanged -= OnEnergyChanged;
            view.EnergyClicked -= OnEnergyClicked;

            model.GoldChanged -= OnGoldChanged;
            view.GoldClicked -= OnGoldClicked;
        }

        public void UpdateCurrentTime(DateTime dateTime, DateTime firstPlayDateTime)
        {
            view.UpdateTime(dateTime, firstPlayDateTime);
        }

        private void OnGoldChanged(int amount)
        {
            view.UpdateGold(amount);
        }

        private void OnGoldClicked(ClickData clickData)
        {
            switch (clickData.Button)
            {
                case PointerEventData.InputButton.Left:
                    model.DecreaseGold(DecreaseGoldAmount);
                    break;
                case PointerEventData.InputButton.Right:
                    model.IncreaseGold(IncreaseGoldAmount);
                    break;
            }
        }

        private void OnHealthChanged(int amount)
        {
            view.UpdateHealth(amount, model.CharacterModel.MaxHealth);
        }

        private void OnHealthClicked(ClickData clickData)
        {
            switch (clickData.Button)
            {
                default:
                    model.CharacterModel.DecreaseHealth(DecreaseHealthAmount);
                    break;
                case PointerEventData.InputButton.Right:
                    model.CharacterModel.IncreaseHealth(IncreaseHealthAmount);
                    break;
                case PointerEventData.InputButton.Middle:
                    model.CharacterModel.RestoreHealth();
                    break;
            }
        }

        private void OnEnergyChanged(int amount)
        {
            view.UpdateEnergy(amount, model.CharacterModel.MaxEnergy);
        }

        private void OnEnergyClicked(ClickData clickData)
        {
            switch (clickData.Button)
            {
                default:
                    model.CharacterModel.DecreaseEnergy(DecreaseEnergyAmount);
                    break;
                case PointerEventData.InputButton.Right:
                    model.CharacterModel.IncreaseEnergy(IncreaseEnergyAmount);
                    break;
                case PointerEventData.InputButton.Middle:
                    model.CharacterModel.RestoreEnergy();
                    break;
            }
        }
    }
}