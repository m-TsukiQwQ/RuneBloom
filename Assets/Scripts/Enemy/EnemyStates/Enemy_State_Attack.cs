using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy_State_Attack : EnemyState
{
    
    public Enemy_State_Attack(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.SetVelocity(0,0);
        _enemy.lastTimeAttacked = Time.time;
    }
    public override void Update()
    {
        base.Update();
        if (_enemy.playerPosition == null ) 
        {
            _stateMachine.ChangeState(_enemy.idleState);
        }
        
        _enemy._enemyAnimations.SetAttackAnimation( _enemy.playerPosition.position - _enemy.transform.position);
        

        if (_triggerCalled)
        {
            
            _stateMachine.ChangeState(_enemy.waitForAttackState);
        }
    }

}
