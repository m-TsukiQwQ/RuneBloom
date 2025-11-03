using UnityEngine;

public class PlayerStateDead : PlayerState
{
    public PlayerStateDead(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.rb.simulated = false;
        _player.SetVelocity(0,0);
        

    }
}
