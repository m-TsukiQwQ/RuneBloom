using UnityEngine;

public class Enemy_State_Chase : EnemyState
{
    private float multiplier;
    
    public Enemy_State_Chase(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Chase state");
        multiplier = enemy.chaseSpeedMultiplier;
        enemy.animator.SetFloat("AnimSpeedMultiplier", (enemy.moveSpeed * multiplier / enemy.moveSpeed));
        


    }
    public override void Update()
    {
        base.Update();

       

        if (!enemy.PlayerDetection() || enemy.playerPosition == null)
        {
            _stateMachine.ChangeState(enemy.idleState);
        }
        else
            ChasePlayer();

        if(enemy.PlayerInAttackRange()) 
            _stateMachine.ChangeState(enemy.attackState);
    }

    private void ChasePlayer()
    {
        Vector3 directionToPlayer = enemy.playerPosition.position - enemy.transform.position;
        Vector3 normalizedDirection = directionToPlayer.normalized;
        if (directionToPlayer.magnitude >= 1f)
        {
            enemy.SetVelocity((normalizedDirection.x * (enemy.moveSpeed * multiplier)), normalizedDirection.y * (enemy.moveSpeed * multiplier));
        }
        
    }

    
}
