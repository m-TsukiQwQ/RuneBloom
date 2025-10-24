using UnityEngine;

public class Enemy_Slime : Enemy
{

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_State_Idle(this, _stateMachine, "Idle");
        wanderState = new Enemy_State_Wander(this, _stateMachine, "Move");

    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Initialize(idleState);
    }
    
}
