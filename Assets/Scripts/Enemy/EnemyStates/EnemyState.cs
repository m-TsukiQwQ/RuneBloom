using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy _enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this._enemy = enemy;
        _rb2d = enemy.rb;
        _animator = enemy.animator;

    }



}
    

