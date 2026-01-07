using System;
using System.Collections;
using UnityEngine;

public class Player : Entity, ISaveable
{
    //states
    public PlayerStateIdle idleState {  get; private set; }
    public PlayerStateRun runState { get; private set; }
    public PlayerStateWalk walkState { get; private set; }
    public PlayerStateAttack attackState { get; private set; }
    public PlayerStateDead deadState {  get; private set; }
    public PlayerStateFreezed freezedState { get; private set; }
    public PlayerStateUseInstrument useInstrumentState { get; private set; }



    //components
    public EntityStatusHandler _statusHandler { get; private set; }
    public PlayerAnimations _playerAnimations { get; private set; }
    public PlayerMovement _playerMovement {  get; private set; }
    public InputHandler inputHandler { get; private set; }

    public float freezeTimer = 2f;

    private UIManager _uiManager;
    private float _originalRunSpeedMultiplier;
    private float animSpeedMultiplier =1;

    public ToolbarController toolbar;

    public static event Action OnPlayerDeath;
    protected override void Awake()
    {

        base.Awake();
        _playerAnimations = GetComponentInChildren<PlayerAnimations>();
        _playerMovement = GetComponent<PlayerMovement>();
        inputHandler = GetComponentInChildren<InputHandler>();
        rb = GetComponent<Rigidbody2D>();
        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        animator.SetFloat("AnimSpeedMultiplier", animSpeedMultiplier);
        _statusHandler = GetComponent<EntityStatusHandler>();   
        toolbar = GetComponentInChildren<ToolbarController>();



        idleState = new PlayerStateIdle(this, _stateMachine, "Idle");
        runState = new PlayerStateRun(this, _stateMachine, "Run");
        walkState = new PlayerStateWalk(this, _stateMachine, "Walk");
        attackState = new PlayerStateAttack(this, _stateMachine, "Attack");
        deadState = new PlayerStateDead(this, _stateMachine, "Dead");
        freezedState = new PlayerStateFreezed(this, _stateMachine, "Freezed");
        useInstrumentState = new PlayerStateUseInstrument(this, _stateMachine, "UseInstrument");



    }
    //AnimSpeedMultiplier

    public override void SlowDownEntity(float duration, float slowMultiplier)
    {
        if (_slowDownCo != null)
            StopCoroutine(_slowDownCo);
        else
        {
            _originalMoveSpeed = _playerMovement.walkSpeed;
            _originalRunSpeedMultiplier = _playerMovement.runSpeedMultiplier;

            _originalMoveAnimSpeedMultiplier = animSpeedMultiplier;

            animator.SetFloat("AnimSpeedMultiplier", animSpeedMultiplier);

        }

        if(_statusHandler._currentChillCharge == 0)
        {
            _playerMovement.walkSpeed = _originalMoveSpeed;
            _playerMovement.runSpeedMultiplier = _originalRunSpeedMultiplier;
            animSpeedMultiplier = _originalMoveAnimSpeedMultiplier;
            animator.SetFloat("AnimSpeedMultiplier", animSpeedMultiplier);

            return;
        }

        _slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        
        float speedMultiplier = 1 - slowMultiplier;

        _playerMovement.walkSpeed *= speedMultiplier;
        _playerMovement.runSpeedMultiplier *= speedMultiplier;
        animSpeedMultiplier *= speedMultiplier;

        animator.SetFloat("AnimSpeedMultiplier", animSpeedMultiplier);

        yield return new WaitForSeconds(duration);

        _playerMovement.walkSpeed = _originalMoveSpeed;
        _playerMovement.runSpeedMultiplier = _originalRunSpeedMultiplier;
        animSpeedMultiplier = _originalMoveAnimSpeedMultiplier;
        animator.SetFloat("AnimSpeedMultiplier", animSpeedMultiplier);
    }

    public override void Freezed()
    {
        base.Freezed();
        _stateMachine.ChangeState(freezedState);
    }

    public void FreezeOverlay()
    {
        _uiManager.ShowCloseFreezeOverlay();
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

    public void LoadData(GameData data)
    {
        if (data.playerPosition != Vector3.zero)
        {
           
            transform.position = data.playerPosition;

            
        }

    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }
}
