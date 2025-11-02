using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlickering : MonoBehaviour
{
    private Light2D _light;
    [SerializeField] private float _minFlickering;
    [SerializeField] private float _maxFlickering;
    [SerializeField] private float _flickeringTime;
    private float _timer;


    private void Awake()
    {
        _light = GetComponentInChildren<Light2D>();
        SetFlickering();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            SetFlickering();
        }




    }

    private void SetFlickering()
    {
        _timer = _flickeringTime;
        float randomFlickering = Random.Range(_minFlickering, _maxFlickering);
        _light.pointLightOuterRadius = randomFlickering;
    }
}
