using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyCombat : EntityCombat
{
    private Enemy _enemy;
    [SerializeField] private Collider2D _checks;

    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponent<Enemy>();
    }
    protected override int GetDominantDirection()
    {
        _attackDirection = _enemy.playerPosition.position - _enemy.transform.position;
        return base.GetDominantDirection();
    }

    public override void PerformAttack()
    {
        base.PerformAttack();

        _checks = GetDetectedColliders();
    }

}
