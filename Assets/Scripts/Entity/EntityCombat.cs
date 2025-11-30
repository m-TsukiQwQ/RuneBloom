using UnityEngine;

public class EntityCombat : MonoBehaviour
{

    private EntityStats _stats;
    private EntityVFX _vFX;

    [Header("Target detection")]
    [SerializeField] protected Transform[] _targetChecks;

    [SerializeField] protected Vector2 _targetCheckRange;
    [SerializeField] protected LayerMask _whatIsTarget;
    protected Vector2 _attackDirection;


    protected virtual void Awake()
    {
        _vFX = GetComponent<EntityVFX>();
        _stats = GetComponent<EntityStats>();
    }
    public virtual void PerformAttack()
    {

        var target = GetDetectedColliders();
        if (target == null) return;
        

            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null) return;

            float elementalDamage = _stats.GetElementalDamage(out ElementType element);
            float physicalDamage = _stats.GetPhysicalDamage(out bool isCrit);
            bool targetGotHit = damageable.TakeDamage(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                ApplyStatusEffect(target.transform, element);

            if (targetGotHit)
                _vFX?.CreateOnHitVFX(target.transform, isCrit, GetCritVFXRotation(GetDominantDirection()));
        


    }

    public void ApplyStatusEffect(Transform target, ElementType element)
    {
        EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();
        if (statusHandler == null)
            return;

        if (element == ElementType.Ice)
        {
            float maximumCharges = _stats.offence.ice.maxSlowDownStacks.GetValue();
            if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice, maximumCharges))
            {
                statusHandler.ApplyChillEffect(_stats.offence.ice.slowDownDuration.GetValue(), //duration
                                              _stats.offence.ice.slowDownMultiplier.GetValue(),//slowMultiplier
                                              maximumCharges); //maximumcharges
            }
        }

        if (element == ElementType.Fire)
        {
            float maximumCharges = _stats.offence.fire.maxBurnStacks.GetValue();
            if (element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire, maximumCharges))
                statusHandler.ApplyBurnEffect(_stats.offence.fire.burnDuration.GetValue(), _stats.offence.fire.burnDamage.GetValue(), maximumCharges);
        }
        if (element == ElementType.Poison)
        {
            float maximumCharges = _stats.offence.poison.maxPoisonStack.GetValue();
            if (element == ElementType.Poison && statusHandler.CanBeApplied(ElementType.Poison, maximumCharges))
            {
                statusHandler.ApplyPoisonEffect(_stats.offence.poison.poisonStackDuration.GetValue(),
                    _stats.offence.poison.healthRegenerationReduction.GetValue(), maximumCharges, _stats.offence.poison.armorCorrosion.GetValue());
            }
        }
    }

    protected Collider2D GetDetectedColliders()
    {
        int index = GetDominantDirection();
        return Physics2D.OverlapBox(_targetChecks[index].position, _targetCheckRange, 0, _whatIsTarget);
    }

    protected virtual int GetDominantDirection() //up = 2, left = 1, down = 1, right = 3
    {


        if (Mathf.Abs(_attackDirection.x) > Mathf.Abs(_attackDirection.y))
        {
            // --- Horizontal is dominant ---
            // If direction.x is positive, cast right. Otherwise, cast left.
            return (_attackDirection.x > 0) ? 3 : 1;
        }
        else
        {
            // --- Vertical is dominant ---
            // If direction.y is positive, cast up. Otherwise, cast down.
            return (_attackDirection.y > 0) ? 2 : 0;
        }


    }

    private int GetCritVFXRotation(int direction) => direction switch
    {
        0 => 270,
        1 => 180,
        2 => 90,
        3 => 0,
        _ => 0  // This is the default case for any other number
    };

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (_targetChecks != null)
        {
            for (int targetCheck = 0; targetCheck < _targetChecks.Length; targetCheck++)
            {
                Gizmos.DrawWireCube(_targetChecks[targetCheck].position, _targetCheckRange);
            }
        }
    }

}
