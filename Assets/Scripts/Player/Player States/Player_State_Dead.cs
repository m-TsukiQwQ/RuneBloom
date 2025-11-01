using UnityEngine;

public class Player_State_Dead : PlayerState
{
    public Player_State_Dead(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.rb.simulated = false;
        _player.SetVelocity(0,0);
        

    }
}
