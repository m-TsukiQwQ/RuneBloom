using UnityEngine;

public class EnemySlime : Enemy
{

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyStateIdle(this, _stateMachine, "Idle");
        wanderState = new EnemyStateWander(this, _stateMachine, "Move");
        chaseState = new EnemyStateChase(this, _stateMachine, "Move");
        attackState = new EnemyStateAttack(this, _stateMachine, "Attack");
        waitForAttackState = new EnemyStateWaitForAttack(this, _stateMachine, "Idle");
        deadState = new EnemyStateDead(this, _stateMachine, "Dead");
        freezedState = new EnemyStateFreezed(this, _stateMachine, "Idle");
        

    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Initialize(idleState);
    }
    
}
