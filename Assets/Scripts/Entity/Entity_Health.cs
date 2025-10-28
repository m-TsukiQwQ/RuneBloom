using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity _entity;

    [Header("Health deatails")]
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected bool IsDead;

    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _currentHealth = _maxHealth;
    }
    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (IsDead) return;
        ReduceHealth(damage);
        
    }

    protected void ReduceHealth(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();

        }
    }

    private void Die()
    {
        IsDead = true;

        _entity.EntityDeath();
    }


}
