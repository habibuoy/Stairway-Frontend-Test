using System;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private const int HoursMultiplier = 24;
        private const float TimeUpdateRate = 1f;

        private static GameManager instance;

        private float timeUpdateTimer;

        public static GameManager Instance => instance;
        public DateTime FirstPlayDateTime { get; private set; }
        public DateTime CurrentTime { get; private set; }

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
        }

        private void OnDestroy()
        {
            CurrentTimeChanged = null;
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
    }
}