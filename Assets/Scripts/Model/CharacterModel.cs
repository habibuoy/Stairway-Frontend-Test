using System;
using UnityEngine;

namespace Game.UI.Model
{
    public class CharacterModel
    {
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Energy { get; private set; }
        public int MaxEnergy { get; private set; }

        public event Action<int> HealthChanged;
        public event Action<int> EnergyChanged;

        public CharacterModel(int maxHealth, int health, int maxEnergy, int energy)
        {
            MaxHealth = maxHealth;
            Health = health;
            MaxEnergy = maxEnergy;
            Energy = energy;
        }

        public void RestoreHealth()
        {
            Health = MaxHealth;
            HealthChanged?.Invoke(Health);
        }

        public void IncreaseHealth(int amount)
        {
            Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
            HealthChanged?.Invoke(Health);
        }

        public void DecreaseHealth(int amount)
        {
            Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
            HealthChanged?.Invoke(Health);
        }

        public void RestoreEnergy()
        {
            Energy = MaxEnergy;
            EnergyChanged?.Invoke(Energy);
        }

        public void IncreaseEnergy(int amount)
        {
            Energy = Mathf.Clamp(Energy + amount, 0, MaxEnergy);
            EnergyChanged?.Invoke(Energy);
        }

        public void DecreaseEnergy(int amount)
        {
            Energy = Mathf.Clamp(Energy - amount, 0, MaxEnergy);
            EnergyChanged?.Invoke(Energy);
        }
    }
}