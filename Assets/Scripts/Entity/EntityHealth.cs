using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityHealth : MonoBehaviour, IDamageable
{

    protected Entity _entity;
    private EntityVFX _entityVfx;
    protected EntityStats _stats;

    [Header("Health deatails")]
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected bool isDead;
    [SerializeField] private bool _canRegenerateHealth = true;
    private Coroutine _regenerationHealthCo;

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

    private void Update()
    {
        _canRegenerateHealth = (_currentHealth < _stats.GetMaxHealth());
        if (_canRegenerateHealth)
            ApplyRegenerationEffect();

    }
    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead) return false;


        Vector2 position = new Vector2(_entity.transform.position.x, _entity.transform.position.y);
        if (AttackEvaded())
        {
            _entityVfx?.ShowDodgeText(position);
            return false;
        }

        EntityStats attackerStats = damageDealer.GetComponent<EntityStats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float armor = _stats.GetArmor(armorReduction);
        float physicalDamageTaken = damage * (1 - armor);

        float resistance = _stats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(physicalDamageTaken, damageDealer);


        ReduceHealth(physicalDamageTaken + elementalDamage);
        return true;

    }



    public void TakeDamageFromHunger(float damage)
    {
        ReduceHealth(damage);
    }

    public virtual void ReduceHealth(float damage)
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

    protected virtual void IncreaseHealth(float healAmount)
    {
        if (isDead)
            return;

        float healthRegenerationMultiplier = Mathf.Max( (_stats.resources.healthRegenerationMultiplier.GetValue() / 100), 0);


        float newHealth = _currentHealth + (healAmount * healthRegenerationMultiplier);
        float maxHealth = _stats.GetMaxHealth();

        _currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);

    }

    private void ApplyRegenerationEffect()
    {
        float regenerationAmount = _stats.resources.healthRegeneration.GetValue();
        
        float regenerationInterval = _stats.resources.healthRegenerationInterval.GetValue();

        if (_canRegenerateHealth && _regenerationHealthCo == null)
            _regenerationHealthCo = StartCoroutine(RegenerateHealthCo(regenerationInterval, regenerationAmount));

    }
    private IEnumerator RegenerateHealthCo(float regenerationInterval, float regenerationAmount)
    {

        IncreaseHealth(regenerationAmount);
        
        yield return new WaitForSeconds(regenerationInterval);
        _regenerationHealthCo = null;

    }

    private void Die()
    {
        isDead = true;

        _entity.EntityDeath();
    }

    private bool AttackEvaded() => Random.Range(1, 100) <= _stats.GetEvasion();

    private bool IsHeavyDamage(float damage) => damage / _stats.GetMaxHealth() >= _heavyDamageTreshhold;


    //knockback logic
    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        Vector2 direction = -((damageDealer.transform.position - _entity.transform.position).normalized);
        Vector2 knockback = IsHeavyDamage(damage) ? _heavyKnockbackPower : _knockbackPower;
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
