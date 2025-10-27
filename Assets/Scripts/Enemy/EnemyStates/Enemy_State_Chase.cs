using UnityEngine;

public class Enemy_State_Chase : EnemyState
{
    private Transform _player;
    private float multiplier;

    private float lastTimePlayerDetected;
    
    public Enemy_State_Chase(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (_player == null) 
            _player = enemy.GetPlayerReference();


        multiplier = enemy.chaseSpeedMultiplier;
        enemy.animator.SetFloat("AnimSpeedMultiplier", (enemy.moveSpeed * multiplier / enemy.moveSpeed));
        


    }
    public override void Update()
    {
        base.Update();

       if(enemy.PlayerDetection() == true)
        {
            UpdateChaseTimer();
        }

        if (/*!enemy.PlayerDetection() ||*/ enemy.playerPosition == null)
        {
            _stateMachine.ChangeState(enemy.idleState);
        }
        else
            ChasePlayer();


        if (ChaseTimeIsOver())
        {
            _stateMachine.ChangeState(enemy.idleState);
        }    
       

        if(enemy.PlayerInAttackRange() && enemy.IsAttackCoolDownIsOver()) 
            _stateMachine.ChangeState(enemy.attackState);

    }

    private void ChasePlayer()
    {
        Vector3 directionToPlayer = enemy.playerPosition.position - enemy.transform.position;
        Vector3 normalizedDirection = directionToPlayer.normalized;
        if (directionToPlayer.magnitude > enemy.attackRange)
        {
            enemy.SetVelocity((normalizedDirection.x * (enemy.moveSpeed * multiplier)), normalizedDirection.y * (enemy.moveSpeed * multiplier));
        }
        
    }

    private void UpdateChaseTimer() => lastTimePlayerDetected = Time.time;

    private bool ChaseTimeIsOver() => Time.time > lastTimePlayerDetected + enemy.chaseTime;

    
}
