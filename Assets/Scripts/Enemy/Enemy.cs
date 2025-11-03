using UnityEngine;

public class Enemy : Entity
{
    //states
    public EnemyStateIdle idleState;
    public EnemyStateWander wanderState;
    public EnemyStateNonBattle nonBattleState;
    public EnemyStateChase chaseState;
    public EnemyStateAttack attackState;
    public EnemyStateWaitForAttack waitForAttackState;
    public EnemyStateDead deadState;

    public EnemyAnimation _enemyAnimations { get; private set; }


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

    [Header("Chase State")]
    public float chaseRange;
    public LayerMask whatIsPlayer;
    public float chaseSpeedMultiplier;
    public bool playerDetected;
    public Transform playerPosition;
    public float chaseTime;
    public Vector3 directionToPlayer;

    [Header("Attack details")]
    public float attackRange;
    public bool playerInAttackRange;
    public float attackCoolDown;
    public float lastTimeAttacked;

    [Header("Death details")]
    [SerializeField] private float deathTimer = 3f;

    protected override void Awake()
    {
        base.Awake();
        _enemyAnimations = GetComponentInChildren<EnemyAnimation>();
    }

    private void HandlePlayerDeath()
    {
        _stateMachine.ChangeState(idleState);
    }

    public Collider2D PlayerDetection()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, chaseRange, whatIsPlayer);
        if (playerCollider != null)
        {
            playerPosition = playerCollider.transform;
        }

        return playerCollider;
    }



    public Transform GetPlayerReference()
    {
        if (playerPosition == null)
        {
            Collider2D player = PlayerDetection();

            if (player != null)
                playerPosition =player.transform;
        }

        return playerPosition;
    }

    public bool IsAttackCoolDownIsOver() => Time.time > lastTimeAttacked + attackCoolDown;

    public override void EntityDeath()
    {
        base.EntityDeath();
        _stateMachine.ChangeState(deadState);
    }
    public void Death()
    {
        Destroy(this.gameObject, deathTimer);
    }
    protected override void Update()
    {
        base.Update();
        moveInput = rb.linearVelocity;
        if (moveInput != Vector2.zero)
        {


            moveDirection = moveInput;

        }
        _enemyAnimations.SetMoveAnimation(moveDirection);
    }

    public bool PlayerInAttackRange()
    {
        if(playerPosition == null) return false;

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
        if (playerCollider != null) return true;



        return false;
    }
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, wanderRange);
        Gizmos.DrawLine(transform.position, wanderPosition);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    { 
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
