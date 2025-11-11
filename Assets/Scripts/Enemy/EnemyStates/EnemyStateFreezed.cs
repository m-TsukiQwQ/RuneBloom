using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyStateFreezed : EnemyState
{
    public EnemyStateFreezed(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Freezed");
        _enemy.SetVelocity(0, 0);
        _enemy.animator.SetFloat("AnimSpeedMultiplier", 0);
        _stateTimer = _enemy.freezeTimer;
        
    }

    public override void Update()
    {
        base.Update();
        if(_stateTimer < 0)
            _stateMachine.ChangeState(_enemy.idleState);

        
        _enemy.animator.SetFloat("AnimSpeedMultiplier", _enemy.moveAnimSpeedMultiplier);
    }

    public override void Exit()
    {
        Debug.Log("Unreezed");
    }

}
