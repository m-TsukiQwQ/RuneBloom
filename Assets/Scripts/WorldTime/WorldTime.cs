using System;
using System.Collections;
using UnityEngine;

namespace WorldTime
{
    public class WorldTime : MonoBehaviour, ISaveable
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;

        [SerializeField] private float _dayLength; //in seconds

        [SerializeField] private int _startHour = 0;
        [SerializeField] private int _startMinute = 0;

        private TimeSpan _currentTime;
        private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

        public TimeSpan CurrentTime => _currentTime;


        private void Awake()
        {
            _currentTime = new TimeSpan(_startHour, _startMinute, 0);
        }

        private void Start()
        {
            StartCoroutine(TimeLoop());
        }
        private IEnumerator TimeLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_minuteLength);
                _currentTime += TimeSpan.FromMinutes(1);
                WorldTimeChanged?.Invoke(this, _currentTime);
            }
        }

        public void LoadData(GameData data)
        {
            // 1. Check if data is valid (Not 0, which implies New Game / Empty Data)
            // 8:00 AM is approx 288,000,000,000 ticks, so > 0 is safe check for New Game.
            if (data.worldTimeTicks > 0)
            {
                _currentTime = new TimeSpan(data.worldTimeTicks);
                Debug.Log($"[WorldTime] Loaded Time: {_currentTime}");

                // 2. Important: Fire event immediately so UI updates instantly on load
                WorldTimeChanged?.Invoke(this, _currentTime);
            }
            else
            {
                Debug.LogWarning("[WorldTime] Save data was empty (0 ticks). Using default 8:00 AM.");
            }
        }

        public void SaveData(ref GameData data)
        {
            
            data.dayCount = (int)_currentTime.TotalDays;
            data.worldTimeTicks = _currentTime.Ticks;
        }
    }
}
