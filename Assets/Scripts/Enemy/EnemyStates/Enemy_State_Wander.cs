using UnityEngine;

public class Enemy_State_Wander : EnemyState
{
    public Enemy_State_Wander(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered " + _animBoolName);
        GetNewDestination();
        enemy.animator.SetFloat("AnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
    }


    public override void Update()
    {
        base.Update();
        
        Wander();
        
        
    }

    private void Wander()
    {
        Vector3 moveDirection = (enemy.wanderPosition - enemy.transform.position).normalized;

        if (Vector3.Distance(enemy.transform.position, enemy.wanderPosition) >= 0.5f)
        {
            enemy.SetVelocity(moveDirection.x * enemy.moveSpeed, moveDirection.y * enemy.moveSpeed);
            if (enemy.edgeDetected)
            {
                Vector3 directionToWall = (enemy.wanderPosition - enemy.transform.position).normalized;
                Vector3 oppositeDirection = -directionToWall;
                float randomDistance = Random.Range(enemy.wanderRange.x * 0.5f, enemy.wanderRange.x);
                enemy.wanderPosition = enemy.transform.position + (oppositeDirection * randomDistance);
            }

        }
        else
            _stateMachine.ChangeState(enemy.idleState);
    }

    private void GetNewDestination()
    {
        float randomX = Random.Range(-enemy.wanderRange.x, enemy.wanderRange.x);
        float randomY = Random.Range(-enemy.wanderRange.y, enemy.wanderRange.y);
        enemy.wanderPosition = enemy.transform.position + new Vector3(randomX, randomY);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exited " + _animBoolName);
    }
}
