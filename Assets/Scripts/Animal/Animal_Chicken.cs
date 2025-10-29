using UnityEngine;

public class Animal_Chicken : Animal
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new Animal_State_Idle(this, _stateMachine, "Idle");
        wanderState = new Animal_State_Wander(this, _stateMachine, "Move");
        
    }
}
