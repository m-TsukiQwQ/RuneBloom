using UnityEngine;

public class ObjectBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected float _maximumHealth;
    [SerializeField] protected float _currentHealth;
    protected Animator _anim;

    public bool TakeDamage(float damage, float ElemetalDamage, ElementType element, Transform damageDealer)
    {
        return false;
    }

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _currentHealth = _maximumHealth;
    }

    public virtual bool TakeDamage(float damage)
    {
        DecreaseHealth(damage);
        if (_anim != null)
            _anim.SetTrigger("Hit");
        return true;
    }

    private void Returnback()
    {
        if (_anim != null)
            _anim.SetTrigger("End");
    }

    protected virtual void DecreaseHealth(float health)
    {
        _currentHealth -= health;
        if (_currentHealth <= 0)
            Destroy(this.gameObject);
    }
}
