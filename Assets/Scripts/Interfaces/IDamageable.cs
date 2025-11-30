using UnityEngine;

public interface IDamageable 
{
    public bool TakeDamage(float damage, float ElemetalDamage, ElementType element, Transform damageDealer);

    public bool TakeDamage(float damage);
    
}
