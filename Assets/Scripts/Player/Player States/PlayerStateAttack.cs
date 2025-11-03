using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStateAttack : PlayerState
{
    private int AttackDirection;
    public PlayerStateAttack(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    

    public override void Enter()
    {
        base.Enter();
        
        _player._playerAnimations.SetIdleAnimation(_player.inputHandler.mouseDirection);
    }
    public override void Update()
    {
        base.Update();
        _player.SetVelocity(0, 0);
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
    

