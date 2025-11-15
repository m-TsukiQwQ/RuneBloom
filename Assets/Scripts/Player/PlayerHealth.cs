using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : EntityHealth
{
    [Header("Health Bar")]
    [SerializeField] private Image _healthBar;

    private void UpdateHealthBar()
    {
        if (_healthBar == null) return;

        _healthBar.fillAmount = _currentHealth / _stats.GetMaxHealth();
    }

    protected override void Awake()
    {
        base.Awake();
        UpdateHealthBar();

    }
    public override void ReduceHealth(float damage)
    {
        base.ReduceHealth(damage);
        UpdateHealthBar();


    }

    protected override void IncreaseHealth(float healAmount)
    {
        base.IncreaseHealth(healAmount);
        UpdateHealthBar();
    }
}
