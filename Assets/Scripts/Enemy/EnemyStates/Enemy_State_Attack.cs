using UnityEngine;

public class Enemy_State_Attack : EnemyState
{
    
    public Enemy_State_Attack(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0,0);
        enemy.lastTimeAttacked = Time.time;
    }
    public override void Update()
    {
        base.Update();
        if (enemy.playerPosition == null ) 
        {
            _stateMachine.ChangeState(enemy.idleState);
        }

        //if (!enemy.PlayerInAttackRange() && enemy.playerPosition != null)
        //{
        //    _stateMachine.ChangeState(enemy.chaseState);
        //}

        if (_triggerCalled)
        {
            _stateMachine.ChangeState(enemy.waitForAttackState);
        }
    }

    

    public override void Exit()
    {
        base.Exit();
        
    }


}
