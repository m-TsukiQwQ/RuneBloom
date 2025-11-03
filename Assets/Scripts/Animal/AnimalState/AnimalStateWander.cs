using UnityEngine;

public class AnimalStateWander : AnimalState
{
    public AnimalStateWander(Animal animal, StateMachine stateMachine, string animBoolName) : base(animal, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GetNewDestination();
        
    }


    public override void Update()
    {
        base.Update();

        Wander();


    }

    private void Wander()
    {
        Vector3 moveDirection = (_animal.wanderPosition - _animal.transform.position).normalized;


        if (Vector3.Distance(_animal.transform.position, _animal.wanderPosition) >= 0.5f)
        {
            _animal.SetVelocity(moveDirection.x * _animal.moveSpeed, moveDirection.y * _animal.moveSpeed);
            if (_animal.edgeDetected)
            {
                Vector3 directionToWall = (_animal.wanderPosition - _animal.transform.position).normalized;
                Vector3 oppositeDirection = -directionToWall;
                float randomDistance = Random.Range(_animal.wanderRange.x * 0.5f, _animal.wanderRange.x);
                _animal.wanderPosition = _animal.transform.position + (oppositeDirection * randomDistance);
            }

        }
        else
            _stateMachine.ChangeState(_animal.idleState);
    }

    private void GetNewDestination()
    {
        float randomX = Random.Range(-_animal.wanderRange.x, _animal.wanderRange.x);
        float randomY = Random.Range(-_animal.wanderRange.y, _animal.wanderRange.y);
        _animal.wanderPosition = _animal.transform.position + new Vector3(randomX, randomY);
    }
}
