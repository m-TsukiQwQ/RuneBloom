using UnityEngine;

public class Enemy_State_NonBattle : EnemyState
{
    public Enemy_State_NonBattle(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();
        if (enemy.PlayerDetection())
        {
            _stateMachine.ChangeState(enemy.chaseState);

        }
        
    }
}
