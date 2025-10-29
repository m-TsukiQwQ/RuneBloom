using UnityEngine;

public class Animal : Entity
{
    public Animal_State_Idle idleState;
    public Animal_State_Wander wanderState;
    private Entity_Animation _anim;

    [Header("General Movement Details")]
    public float idleTime = 2f;
    public float moveSpeed;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;
    public Vector2 moveInput;
    public Vector2 moveDirection;


    [Header("Wander State")]
    public float wanderTime;
    public Vector2 wanderRange;
    [HideInInspector]
    public Vector3 wanderPosition;


    override protected void Awake()
    {
        base.Awake();
        _anim = GetComponentInChildren<Entity_Animation>();
    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        moveInput = rb.linearVelocity;
        if (moveInput != Vector2.zero)
        {


            moveDirection = moveInput;

        }
        _anim.SetMoveAnimation1D(moveDirection);
    }


    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, wanderRange);
        Gizmos.DrawLine(transform.position, wanderPosition);


    }
}
