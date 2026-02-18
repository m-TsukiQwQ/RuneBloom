using UnityEngine;

public class AnimalStateDead : AnimalState
{
    public AnimalStateDead(Animal animal, StateMachine stateMachine, string animBoolName) : base(animal, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _animal.GetComponentInChildren<Collider2D>().enabled = false;
        _animal.GetComponent<LootSpawner>()?.SpawnLoot();

        _animal.Death();
        _stateMachine.SwithOffMachine();
    }
}
