using System;
using System.Text;
using Game.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.View.Inventory
{
    public class GeneralInventoryView : BaseInventoryView
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI energyText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private Image healthBar;
        [SerializeField] private Image energyBar;
        [SerializeField] private Clickable goldClickable;
        [SerializeField] private Clickable healthClickable;
        [SerializeField] private Clickable energyClickable;

        public event Action<ClickData> GoldClicked;
        public event Action<ClickData> HealthClicked;
        public event Action<ClickData> EnergyClicked;

        public override void Initialize()
        {
            goldClickable.Clicked += OnGoldClicked;
            healthClickable.Clicked += OnHealthClicked;
            energyClickable.Clicked += OnEnergyClicked;
        }

        private void OnDestroy()
        {
            goldClickable.Clicked -= OnGoldClicked;
            healthClickable.Clicked -= OnHealthClicked;
            energyClickable.Clicked -= OnEnergyClicked;
        }

        private void OnGoldClicked(Clickable clickable, ClickData data)
        {
            GoldClicked?.Invoke(data);
        }

        private void OnEnergyClicked(Clickable clickable, ClickData clickData)
        {
            EnergyClicked?.Invoke(clickData);
        }

        private void OnHealthClicked(Clickable clickable, ClickData clickData)
        {
            HealthClicked?.Invoke(clickData);
        }

        public void UpdateTime(DateTime dateTime, DateTime firstPlayDateTime)
        {
            string season = dateTime.AsSeason().AsString();
            string year = (dateTime.Year - firstPlayDateTime.Year + 1).ToOrdinal();
            timeText.text = $"{year} Year\n{season} {dateTime.Day}";
        }

        public void UpdateGold(int gold)
        {
            goldText.text = gold.ToString();
        }
        
        public void UpdateHealth(int health, int maxHealth)
        {
            healthText.text = $"{health} / {maxHealth}";
            healthBar.fillAmount = (float) health / maxHealth;
        }

        public void UpdateEnergy(int energy, int maxEnergy)
        {
            energyText.text = $"{energy} / {maxEnergy}";
            energyBar.fillAmount = (float) energy / maxEnergy;
        }
    }
}