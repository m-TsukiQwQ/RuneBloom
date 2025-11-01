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

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            double totalDays = newTime.TotalDays;
            int dayNumber = (int)totalDays;
            _timeText.SetText(newTime.ToString(@"hh\:mm"));
            _dayText.SetText("Day  " + (dayNumber + 1));
        }

        private void OnDisable()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }
    }

}
