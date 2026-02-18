using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditorInternal;
using UnityEngine;

public class AnimalCow : Animal
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new AnimalStateIdle(this, _stateMachine, "Idle");
        wanderState = new AnimalStateWander(this, _stateMachine, "Move");
        deadState = new AnimalStateDead(this, _stateMachine, "Idle");

    }
}
