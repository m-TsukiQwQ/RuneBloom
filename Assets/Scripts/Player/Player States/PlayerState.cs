using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player _player;
    protected PlayerInputSystem _inputSystem;
    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base ( stateMachine, animBoolName)
    {
        this._player = player;

        _animator = _player.animator;
        _inputSystem = _player._playerMovement.input;
        _rb2d = _player.rb;
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
