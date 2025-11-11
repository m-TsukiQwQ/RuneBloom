using System.Collections;
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
    public EnemyStateFreezed freezedState;

    public EnemyAnimation _enemyAnimations { get; private set; }

    [Header("State")]
    [SerializeField] private EntityState _currentState;

    [Header("General Movement Details")]
    public float idleTime = 2f;
    public float moveSpeed;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector2 moveDirection;


    [Header("Wander State")]
    public float wanderTime;
    public Vector2 wanderRange;
    [HideInInspector] public Vector3 wanderPosition;

    [Header("Chase State")]
    public float chaseRange;
    public LayerMask whatIsPlayer;
    public float chaseSpeedMultiplier;
    [HideInInspector] public bool playerDetected;
    [HideInInspector] public Transform playerPosition;
    public float chaseTime;
    [HideInInspector] public Vector3 directionToPlayer;

    [Header("Attack details")]
    public float attackRange;
    [HideInInspector] public bool playerInAttackRange;
    public float attackCoolDown;
    [HideInInspector] public float lastTimeAttacked;

    [Header("Death details")]
    [SerializeField] private float deathTimer = 3f;

    [Header("Freeze details")]
    public float freezeTimer = 2f;

    private float _originalChaseSpeedMultiplier;
    protected override void Awake()
    {
        base.Awake();
        _enemyAnimations = GetComponentInChildren<EnemyAnimation>();
        _currentState = _stateMachine.currentState;
        animator.SetFloat("AnimSpeedMultiplier", moveAnimSpeedMultiplier);
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

        _currentState = _stateMachine.currentState;
    }

    public override void Freezed()
    {
        base.Freezed();
        _stateMachine.ChangeState(freezedState);
    }

    public override void SlowDownEntity(float duration, float slowMultiplier)
    {
        if (_slowDownCo != null)
            StopCoroutine(_slowDownCo);
        else
        {
            _originalMoveSpeed = moveSpeed;
            _originalMoveAnimSpeedMultiplier = moveAnimSpeedMultiplier;
            _originalChaseSpeedMultiplier = chaseSpeedMultiplier;
            animator.SetFloat("AnimChaseSpeedMultiplier", chaseSpeedMultiplier);
            animator.SetFloat("AnimSpeedMultiplier", moveAnimSpeedMultiplier);
        }

        _slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float speedMultiplier = 1 - slowMultiplier;
        chaseSpeedMultiplier *= speedMultiplier;
        moveSpeed *= speedMultiplier;
        moveAnimSpeedMultiplier *= speedMultiplier;
        
        animator.SetFloat("AnimChaseSpeedMultiplier", chaseSpeedMultiplier);
        animator.SetFloat("AnimSpeedMultiplier", moveAnimSpeedMultiplier);

        yield return new WaitForSeconds(duration);

        moveSpeed = _originalMoveSpeed;
        moveAnimSpeedMultiplier = _originalMoveAnimSpeedMultiplier;
        chaseSpeedMultiplier = _originalChaseSpeedMultiplier;
        animator.SetFloat("AnimChaseSpeedMultiplier", chaseSpeedMultiplier);
        animator.SetFloat("AnimSpeedMultiplier", moveAnimSpeedMultiplier);
    }

    public bool IsAttackCoolDownIsOver() => Time.time > lastTimeAttacked + attackCoolDown;


    public bool PlayerInAttackRange()
    {
        if (playerPosition == null) return false;

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
        if (playerCollider != null) return true;



        return false;
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
                playerPosition = player.transform;
        }

        return playerPosition;
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


    public override void EntityDeath()
    {
        base.EntityDeath();
        _stateMachine.ChangeState(deadState);
    }
    public void Death()
    {
        Destroy(this.gameObject, deathTimer);
    }

    private void HandlePlayerDeath()
    {
        _stateMachine.ChangeState(idleState);
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
