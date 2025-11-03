using System;
using UnityEngine;

public class Player : Entity
{
    //states
    public PlayerStateIdle idleState {  get; private set; }
    public PlayerStateRun runState { get; private set; }
    public PlayerStateWalk walkState { get; private set; }
    public PlayerStateAttack attackState { get; private set; }
    public PlayerStateDead deadState {  get; private set; }



    //components
    
    public PlayerAnimations _playerAnimations { get; private set; }
    public PlayerMovement _playerMovement {  get; private set; }
    public InputHandler inputHandler { get; private set; }


    public static event Action OnPlayerDeath;
    protected override void Awake()
    {

        base.Awake();
        
        _playerAnimations = GetComponentInChildren<PlayerAnimations>();
        _playerMovement = GetComponent<PlayerMovement>();
        inputHandler = GetComponentInChildren<InputHandler>();
        rb = GetComponent<Rigidbody2D>();




        idleState = new PlayerStateIdle(this, _stateMachine, "Idle");
        runState = new PlayerStateRun(this, _stateMachine, "Run");
        walkState = new PlayerStateWalk(this, _stateMachine, "Walk");
        attackState = new PlayerStateAttack(this, _stateMachine, "Attack");
        deadState = new PlayerStateDead(this, _stateMachine, "Dead");



    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Initialize(idleState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeath?.Invoke();
        _stateMachine.ChangeState(deadState);

    }

    
}
