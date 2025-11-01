using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamageable
{
    protected Entity _entity;
    private EntityVFX _entityVfx;
    protected EntityStats _stats;

    [Header("Health deatails")]
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected bool IsDead;


    protected virtual void Awake()
    {
        _entity = GetComponent<Entity>();
        _stats = GetComponent<EntityStats>();
        _entityVfx = GetComponent<EntityVFX>();

        _currentHealth = _stats.GetMaxHealth();
    }
    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (IsDead) return;
        _entityVfx?.PlayOnDamageVfx();
        ReduceHealth(damage);
        
    }

    protected virtual void ReduceHealth(float damage)
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
