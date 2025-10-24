using UnityEngine;

public class Enemy_State_Idle : EnemyState
{
    public Enemy_State_Idle(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered " + _animBoolName);
        base.Enter();
        _stateTimer = enemy.idleTime;
        enemy.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(enemy.wanderState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exited " + _animBoolName);
    }




    }
