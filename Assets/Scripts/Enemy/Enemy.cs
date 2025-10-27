using UnityEngine;

public class Enemy : Entity
{
    //states
    public Enemy_State_Idle idleState;
    public Enemy_State_Wander wanderState;
    public Enemy_State_NonBattle nonBattleState;
    public Enemy_State_Chase chaseState;
    public Enemy_State_Attack attackState;
    public Enemy_State_WaitForAttack waitForAttackState;

    public Enemy_Animation _enemyAnimations { get; private set; }


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

    [Header("Attack details")]
    public float attackRange;
    public bool playerInAttackRange;
    public float attackCoolDown;
    public float lastTimeAttacked;

    protected override void Awake()
    {
        base.Awake();
        _enemyAnimations = GetComponentInChildren<Enemy_Animation>();
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
}
