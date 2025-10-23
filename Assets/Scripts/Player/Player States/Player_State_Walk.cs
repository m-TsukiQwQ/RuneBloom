using UnityEngine;

public class Player_State_Walk : PlayerState
{
    private float speedMultiplier = 1;
    public Player_State_Walk(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (_player._playerMovement.input.Player.Run.IsPressed())
        {
            _stateMachine.ChangeState(_player.runState);
        }

        Move(speedMultiplier);

    }

    private protected void Move(float runSpeedMultiplier)
    {
        
        if (_player._playerMovement.moveInput == Vector2.zero)
        {
            _stateMachine.ChangeState(_player.idleState);
        }

        _player._playerMovement.SetVelocity(_player._playerMovement.moveInput.x * _player._playerMovement.walkSpeed * runSpeedMultiplier,
                                                    _player._playerMovement.moveInput.y * _player._playerMovement.walkSpeed * runSpeedMultiplier);
        _player._playerAnimations.SetMoveAnimation(_player._playerMovement.moveInput);

    }

}
