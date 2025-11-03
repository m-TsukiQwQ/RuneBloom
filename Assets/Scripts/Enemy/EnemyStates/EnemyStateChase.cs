using UnityEngine;

public class EnemyStateChase : EnemyState
{
    private Transform _player;
    private float multiplier;
    

    private float lastTimePlayerDetected;
    
    public EnemyStateChase(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (_player == null) 
            _player = _enemy.GetPlayerReference();


        multiplier = _enemy.chaseSpeedMultiplier;
        _enemy.animator.SetFloat("AnimSpeedMultiplier", (_enemy.moveSpeed * multiplier / _enemy.moveSpeed));
        


    }
    public override void Update()
    {
        base.Update();

       if(_enemy.PlayerDetection() == true)
        {
            UpdateChaseTimer();
        }

        if (/*!enemy.PlayerDetection() ||*/ _enemy.playerPosition == null)
        {
            _stateMachine.ChangeState(_enemy.idleState);
        }
        else
            ChasePlayer();


        if (ChaseTimeIsOver())
        {
            _stateMachine.ChangeState(_enemy.idleState);
        }    
       

        if(_enemy.PlayerInAttackRange() && _enemy.IsAttackCoolDownIsOver()) 
            _stateMachine.ChangeState(_enemy.attackState);

    }

    private void ChasePlayer()
    {
        Vector3 directionToPlayer = _enemy.playerPosition.position - _enemy.transform.position;
        Vector3 normalizedDirection = directionToPlayer.normalized;
        _enemy.directionToPlayer = normalizedDirection;
        if (directionToPlayer.magnitude > _enemy.attackRange)
        {
            _enemy.SetVelocity((normalizedDirection.x * (_enemy.moveSpeed * multiplier)), normalizedDirection.y * (_enemy.moveSpeed * multiplier));
        }
        
    }

    private void UpdateChaseTimer() => lastTimePlayerDetected = Time.time;

    private bool ChaseTimeIsOver() => Time.time > lastTimePlayerDetected + _enemy.chaseTime;

    
}
