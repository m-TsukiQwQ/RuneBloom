using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : EntityHealth, ISaveable
{
    [Header("Health Bar")]
    [SerializeField] private Image _healthBar;
    public float CurrentHealth => _currentHealth;

    private void UpdateHealthBar()
    {
        if (_healthBar == null) return;

        _healthBar.fillAmount = _currentHealth / GetMaxHealth();
    }

    public float GetMaxHealth()
    {
        return _stats.GetMaxHealth();
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

    public override void IncreaseHealth(float healAmount)
    {
        base.IncreaseHealth(healAmount);
        UpdateHealthBar();
    }

    public void LoadData(GameData data)
    {
        _currentHealth = data.playerStats.currentHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.playerStats.currentHealth = _currentHealth;
    }
}
