using UnityEngine;

public class EnemyStateIdle : EnemyStateNonBattle
{
    public EnemyStateIdle(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        
        base.Enter();
        _stateTimer = _enemy.idleTime;
        _enemy.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(_enemy.wanderState);
        }
    }


    }
