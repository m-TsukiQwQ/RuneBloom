using UnityEngine;

public class Enemy_State_WaitForAttack : EnemyState
{
    private int waitDuration;
    public Enemy_State_WaitForAttack(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = enemy.attackCoolDown;
        enemy.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();

        if(_stateTimer < 0)
        {
            _stateMachine.ChangeState(enemy.chaseState);
        }
    }
}
