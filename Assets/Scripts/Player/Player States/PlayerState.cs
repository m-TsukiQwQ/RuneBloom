using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player _player;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base ( stateMachine, animBoolName)
    {
        this._player = player;

        _animator = _player.animator;
    }

   

    public override void Update()
    {
        base.Update();
        if (_stateMachine.currentState == _player.idleState || _stateMachine.currentState == _player.runState || _stateMachine.currentState == _player.walkState)
        {
            if (_player._playerMovement.input.Player.Attack.WasPressedThisFrame())
            {
                _stateMachine.ChangeState(_player.attackState);
            }
        }
    }
}
