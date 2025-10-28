using UnityEngine;

public class Enemy_Slime : Enemy
{

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_State_Idle(this, _stateMachine, "Idle");
        wanderState = new Enemy_State_Wander(this, _stateMachine, "Move");
        chaseState = new Enemy_State_Chase(this, _stateMachine, "Move");
        attackState = new Enemy_State_Attack(this, _stateMachine, "Attack");
        waitForAttackState = new Enemy_State_WaitForAttack(this, _stateMachine, "Idle");
        deadState = new Enemy_State_Dead(this, _stateMachine, "Dead");
        

    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Initialize(idleState);
    }
    
}
