using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using System;

namespace WorldTime
{
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour
    {
        private Light2D _light;

        [SerializeField] private WorldTime _worldTime;

        [Header("Light Settings")]
        [SerializeField] private Gradient _gradient;
        [SerializeField] private AnimationCurve intensityCurve;


        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _worldTime.WorldTimeChanged += OnWorldTimeChanged;

        }


        void Update()
        {
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            _light.color = _gradient.Evaluate(PercentOfDay(newTime));

            _light.intensity = intensityCurve.Evaluate(PercentOfDay(newTime));

        }

        private float PercentOfDay(TimeSpan timeSpan)
        {
            return (float) timeSpan.TotalMinutes % WorldTimeConstants.MinutesInDay / WorldTimeConstants.MinutesInDay;
        }

        private void OnDisable()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

    }
}