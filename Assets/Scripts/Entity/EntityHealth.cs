using System;
using System.Net.NetworkInformation;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EntityHealth : MonoBehaviour, IDamageable
{
    
    protected Entity _entity;
    private EntityVFX _entityVfx;
    protected EntityStats _stats;

    [Header("Health deatails")]
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected bool IsDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 _knockbackPower = new Vector2(1, 1);
    [SerializeField] private Vector2 _heavyKnockbackPower = new Vector2(1, 1);
    [SerializeField] private float _knockbackDuration;
    [SerializeField] private float _heavyKnockbackDuration;
    [SerializeField] private float _heavyDamageTreshhold = .3f; //percentage of health you should lose to consider damage heavy

    protected virtual void Awake()
    {
        _entity = GetComponent<Entity>();
        _stats = GetComponent<EntityStats>();
        _entityVfx = GetComponent<EntityVFX>();

        _currentHealth = _stats.GetMaxHealth();
    }
    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (IsDead) return false;


        Vector2 position = new Vector2(_entity.transform.position.x, _entity.transform.position.y);
        if (AttackEvaded())
        {
            _entityVfx?.ShowDodgeText(position);
            return false;
        }

        float resistance = _stats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damage, damageDealer);


        ReduceHealth(damage + elementalDamage);
        return true;

    }

   

    public void TakeDamageFromHunger(float damage)
    {
        ReduceHealth(damage);
    }

    protected virtual void ReduceHealth(float damage)
    {
        Vector2 position = new Vector2(_entity.transform.position.x, _entity.transform.position.y);
        _currentHealth -= damage;
        _entityVfx?.ShowDamageText(damage, position);
        _entityVfx?.PlayOnDamageVfx();
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

    private bool AttackEvaded() => Random.Range(1, 100) <= _stats.GetEvasion();

    private bool IsHeavyDamage(float damage) => damage / _stats.GetMaxHealth() >= _heavyDamageTreshhold;


    //knockback logic
    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        Vector2 direction = -((damageDealer.transform.position - _entity.transform.position).normalized);
        Vector2 knockback = IsHeavyDamage(damage)? _heavyKnockbackPower : _knockbackPower;
        knockback.x *= direction.x;
        knockback.y *= direction.y;

        return knockback;

    }

    private float CalculateKnockbackDuration(float damage) => IsHeavyDamage(damage) ? _heavyKnockbackDuration : _knockbackDuration;
   
     private void TakeKnockback(float damage, Transform damageDealer)
        {
            Vector2 knockback = CalculateKnockback(damage, damageDealer);
            _entity?.RecieveKnockback(knockback, CalculateKnockbackDuration(damage));
        }

   


}
