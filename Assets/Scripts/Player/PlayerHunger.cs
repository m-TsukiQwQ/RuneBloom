using UnityEngine;
using UnityEngine.UI;

public class PlayerHunger : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private EntityStats _stats;
    [Header("Hunger Bar")]
    [SerializeField] private Image _hungerBar;
    [SerializeField]
    private float _currentHunger;
    [SerializeField]
    private float _hungerSpeed;
    [SerializeField]
    private float _hungerPower;
    [SerializeField]
    private float _hungerTimer;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _stats = GetComponent<EntityStats>();

        _currentHunger = _stats.GetMaxHunger();
    }

    private void FixedUpdate()
    {
        _hungerTimer -= Time.fixedDeltaTime;
        if (_hungerTimer <= 0)
        {
            Hunger();
        }
    }

    private void Hunger ()
    {

            _hungerTimer = _hungerSpeed;
            if (_currentHunger > 0)
            {
                _currentHunger -= _hungerPower;
                UpdateHungerBar();
            }
            else
            {
                _playerHealth.TakeDamageFromHunger(1);
            }


    }

    private void UpdateHungerBar()
    {
        if (_hungerBar == null) return;

        _hungerBar.fillAmount = _currentHunger / _stats.GetMaxHunger();
    }
}
