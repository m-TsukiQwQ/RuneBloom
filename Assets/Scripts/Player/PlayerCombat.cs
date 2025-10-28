using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerCombat : EntityCombat
{
    private Player _player;
    [SerializeField] private Collider2D[] _checks;


    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<Player>();

    }

    protected override int GetDominantDirection()
    {
        _attackDirection = _player.inputHandler.mouseDirection;
        return base.GetDominantDirection();
    }

    public override void PerformAttack()
    {
        base.PerformAttack();
        
        _checks = GetDetectedColliders();
    }
    
    
}
