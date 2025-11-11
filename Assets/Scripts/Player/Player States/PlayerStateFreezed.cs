using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateFreezed : PlayerState
{
    public PlayerStateFreezed(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetVelocity(0,0);
        _stateTimer = _player.freezeTimer;
        _player.FreezeOverlay();
    }

    public override void Update()
    {
        base.Update();
        if(_stateTimer < 0)
            _stateMachine.ChangeState(_player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        _player.FreezeOverlay();
    }
    }
