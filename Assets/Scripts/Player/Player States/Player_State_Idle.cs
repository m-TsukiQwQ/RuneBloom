using UnityEngine;

public class Player_State_Idle : PlayerState
{

    public Player_State_Idle(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player._playerMovement.SetVelocity(0, 0);
    }
    public override void Update()
    {
        base.Update();

        if (_player._playerMovement.moveInput != Vector2.zero)
        {
            _stateMachine.ChangeState(_player.walkState);
        }
        _player._playerAnimations.SetIdleAnimation(_player._playerMovement.idleInput);
    }
}
