using UnityEngine;

public class Player : Entity
{
    //states
    public Player_State_Idle idleState {  get; private set; }
    public Player_State_Run runState { get; private set; }
    public Player_State_Walk walkState { get; private set; }
    public Player_State_Attack attackState { get; private set; }



    //components
    
    public Player_Animations _playerAnimations { get; private set; }
    public Player_Movement _playerMovement {  get; private set; }
    public InputHandler inputHandler { get; private set; }


    protected override void Awake()
    {

        base.Awake();
        
        _playerAnimations = GetComponentInChildren<Player_Animations>();
        _playerMovement = GetComponent<Player_Movement>();
        inputHandler = GetComponentInChildren<InputHandler>();


        idleState = new Player_State_Idle(this, _stateMachine, "Idle");
        runState = new Player_State_Run(this, _stateMachine, "Run");
        walkState = new Player_State_Walk(this, _stateMachine, "Walk");
        attackState = new Player_State_Attack(this, _stateMachine, "Attack");

    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Initialize(idleState);
    }
}
