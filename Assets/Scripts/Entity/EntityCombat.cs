using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    public float damage = 5;

    [Header("Target detection")]
    [SerializeField] protected Transform[] _targetChecks;
    [SerializeField] protected Vector2 _targetCheckRange;
    [SerializeField] protected LayerMask _whatIsTarget;
    protected Vector2 _attackDirection;
    protected virtual void Awake()
    {

    }
    public virtual void PerformAttack()
    {
            
        foreach(var target in GetDetectedColliders())
        {
            
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null) continue;

            if(damageable != null) 
                damageable.TakeDamage(damage,transform);
        }
        
        
    }

    protected Collider2D[] GetDetectedColliders()
    {
        int index = GetDominantDirection();
        return Physics2D.OverlapBoxAll(_targetChecks[index].position, _targetCheckRange, 0, _whatIsTarget);
    }

    protected virtual int GetDominantDirection() //up = 0, left = 1, up = 2, right = 3
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (_targetChecks  != null)
            {for (int targetCheck = 0; targetCheck < _targetChecks.Length; targetCheck++)
                {
                    Gizmos.DrawWireCube(_targetChecks[targetCheck].position, _targetCheckRange);
                }
        }
    }

}
