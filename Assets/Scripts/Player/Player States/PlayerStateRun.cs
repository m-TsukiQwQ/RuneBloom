using UnityEngine;

public class PlayerStateRun : PlayerStateWalk
{
    float speedMultiplier;


    public PlayerStateRun(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {

    }
    public override void Enter()
    {
        base.Enter();

        speedMultiplier = _player._playerMovement.runSpeedMultiplier;
    }

  
    public override void Update()
    {
        if (!_player._playerMovement.input.Player.Run.IsPressed())
        {
            _stateMachine.ChangeState(_player.walkState);
        }

        Move(speedMultiplier);
    }
}
