using UnityEngine;

public class AnimalStateIdle : AnimalState
{
    public AnimalStateIdle(Animal animal, StateMachine stateMachine, string animBoolName) : base(animal, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {

        base.Enter();
        _stateTimer = _animal.idleTime;
        _animal.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(_animal.wanderState);
        }
    }


}
