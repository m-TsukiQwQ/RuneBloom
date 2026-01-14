using System;
using TMPro;
using UnityEngine;


namespace WorldTime
{

    [RequireComponent(typeof(TMP_Text))]
    public class WorldTimeDisplay : MonoBehaviour
    {
        

        [SerializeField] private WorldTime _worldTime;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _dayText;

        private void Awake()
        {
            _worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void Start()
        {
            // FIX: Initialize the text immediately on startup.
            // This ensures we see "08:00" instantly instead of waiting for the first minute to tick.
            if (_worldTime != null)
            {
                UpdateText(_worldTime.CurrentTime);
            }
        }

        private void UpdateText(TimeSpan time)
        {
            double totalDays = time.TotalDays;
            int dayNumber = (int)totalDays;

            // Format to 24h clock (08:00)
            _timeText.SetText(time.ToString(@"hh\:mm"));
            _dayText.SetText("Day " + (dayNumber + 1));
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            UpdateText(newTime);
        }

        private void OnDisable()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }
    }

}
