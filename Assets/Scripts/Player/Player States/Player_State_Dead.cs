using UnityEngine;

public class Player_State_Dead : PlayerState
{
    public Player_State_Dead(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        Debug.Log("Dead1 ");
        base.Enter();
        Debug.Log("Dead2 ");
        Debug.Log("Rigidbody simulation set to: " + _rb2d.simulated);
        _player.rb.simulated = false;
        Debug.Log("Dead4 ");
        Debug.Log("Rigidbody simulation set to: " + _rb2d.simulated);
        _player.SetVelocity(0,0);
        Debug.Log("Dead3 ");

    }
}
