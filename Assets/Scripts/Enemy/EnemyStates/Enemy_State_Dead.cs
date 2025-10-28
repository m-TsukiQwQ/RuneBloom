using UnityEngine;

public class Enemy_State_Dead : EnemyState
{
    public Enemy_State_Dead(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.GetComponentInChildren<Collider2D>().enabled = false;

        _enemy.Death();
        _stateMachine.SwithOffMachine();
    }


}
