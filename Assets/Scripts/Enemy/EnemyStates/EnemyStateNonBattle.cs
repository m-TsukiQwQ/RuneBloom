using UnityEngine;

public class EnemyStateNonBattle : EnemyState
{
    public EnemyStateNonBattle(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();
        if (_enemy.PlayerDetection())
        {
            _stateMachine.ChangeState(_enemy.chaseState);

        }
        
    }
}
