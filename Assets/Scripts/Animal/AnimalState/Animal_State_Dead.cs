using UnityEngine;

public class Animal_State_Dead : AnimalState
{
    public Animal_State_Dead(Animal animal, StateMachine stateMachine, string animBoolName) : base(animal, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _animal.GetComponentInChildren<Collider2D>().enabled = false;

        _animal.Death();
        _stateMachine.SwithOffMachine();
    }
}
