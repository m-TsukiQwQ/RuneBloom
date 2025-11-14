using UnityEngine;

public class EnemyStateWander : EnemyStateNonBattle
{

    public EnemyStateWander(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        GetNewDestination();
        _enemy.animator.SetFloat("AnimSpeedMultiplier", _enemy.moveAnimSpeedMultiplier);
    }


    public override void Update()
    {
        base.Update();
        
        Wander();
        
        
    }

    private void Wander()
    {
        Vector3 moveDirection = (_enemy.wanderPosition - _enemy.transform.position).normalized ;
        

        if (Vector3.Distance(_enemy.transform.position, _enemy.wanderPosition) >= 0.5f)
        {
            _enemy.SetVelocity(moveDirection.x * _enemy.moveSpeed, moveDirection.y * _enemy.moveSpeed);
            if (_enemy.edgeDetected)
            {
                Vector3 directionToWall = (_enemy.wanderPosition - _enemy.transform.position).normalized;
                Vector3 oppositeDirection = -directionToWall;
                float randomDistance = Random.Range(_enemy.wanderRange.x * 0.5f, _enemy.wanderRange.x);
                _enemy.wanderPosition = _enemy.transform.position + (oppositeDirection * randomDistance) + new Vector3(0, 0.2f);
            }

        }
        else
            _stateMachine.ChangeState(_enemy.idleState);
    }

    private void GetNewDestination()
    {
        float randomX = Random.Range(-_enemy.wanderRange.x, _enemy.wanderRange.x);
        float randomY = Random.Range(-_enemy.wanderRange.y, _enemy.wanderRange.y);
        _enemy.wanderPosition = _enemy.transform.position + new Vector3(randomX, randomY) + new Vector3(0, 0.2f);
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}
