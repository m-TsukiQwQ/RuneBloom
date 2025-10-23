using UnityEngine;
using UnityEngine.EventSystems;

public class Player_State_Attack : PlayerState
{
    private int AttackDirection;
    public Player_State_Attack(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    

    public override void Enter()
    {
        base.Enter();
        Debug.Log(_player.inputHandler.mouseDirection + " from attack");
        _player._playerAnimations.SetIdleAnimation(_player.inputHandler.mouseDirection);
    }
    public override void Update()
    {
        base.Update();
        _player._playerMovement.SetVelocity(0, 0);
        if (_triggerCalled)
        {
            ManualStateExit();
        }
    }
    private void ManualStateExit()
    {
        _stateMachine.ChangeState(_player.idleState);
    }


   
}
    

