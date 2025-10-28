using System;
using UnityEngine;

public class Player : Entity
{
    //states
    public Player_State_Idle idleState {  get; private set; }
    public Player_State_Run runState { get; private set; }
    public Player_State_Walk walkState { get; private set; }
    public Player_State_Attack attackState { get; private set; }
    public Player_State_Dead deadState {  get; private set; }



    //components
    
    public Player_Animations _playerAnimations { get; private set; }
    public Player_Movement _playerMovement {  get; private set; }
    public InputHandler inputHandler { get; private set; }


    public static event Action OnPlayerDeath;
    protected override void Awake()
    {

        base.Awake();
        
        _playerAnimations = GetComponentInChildren<Player_Animations>();
        _playerMovement = GetComponent<Player_Movement>();
        inputHandler = GetComponentInChildren<InputHandler>();
        rb = GetComponent<Rigidbody2D>();




        idleState = new Player_State_Idle(this, _stateMachine, "Idle");
        runState = new Player_State_Run(this, _stateMachine, "Run");
        walkState = new Player_State_Walk(this, _stateMachine, "Walk");
        attackState = new Player_State_Attack(this, _stateMachine, "Attack");
        deadState = new Player_State_Dead(this, _stateMachine, "Dead");



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
