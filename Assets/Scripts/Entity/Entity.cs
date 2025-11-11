using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{
    protected StateMachine _stateMachine;


    public Rigidbody2D rb;
    private EntityHealth _health;
    public Animator animator { get; private set; }

    [Header("Collision Detection")]
    [SerializeField] private float edgeXCheckDistance;
    [SerializeField] private float edgeYCheckDistance;
    [SerializeField] protected LayerMask whatIsEdge;
    [SerializeField] private Transform xCheck;
    [SerializeField] private Transform yCheck;
    public bool edgeDetected {  get; private set; }

    //knockBack
    private bool _isKnocked;
    private Coroutine _knockbackCo;

    //SlowEffect
    protected Coroutine _slowDownCo;
    protected float _originalMoveSpeed;
    protected float _originalMoveAnimSpeedMultiplier;

    protected bool _gotHit = false;
    


    protected virtual void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        _stateMachine = new StateMachine();
        _health = GetComponent<EntityHealth>();

    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        _stateMachine.UpdateActiveState();
        HandleCollision();

    }
    

    public virtual void Freezed()
    {

    }

    public virtual void EntityDeath()
    {

    }

    public virtual void SlowDownEntity(float duration, float slowMultiplier)
    {
        
    }

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }

    public void RecieveKnockback(Vector2 knockback, float duration)
    {
        if(_knockbackCo != null )
        {
            StopCoroutine( _knockbackCo );

        }

        _knockbackCo = StartCoroutine (KnockbackCo(knockback, duration));

    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        _isKnocked = true;
        rb.linearVelocity = knockback;
        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        _isKnocked = false;
    }

    public void CallAnimationTrigger()
    {
        _stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (_isKnocked) return;
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    }
    
    private void HandleCollision()
    {
        if (xCheck != null && yCheck != null)
        {
            edgeDetected = Physics2D.Raycast(xCheck.position, Vector2.right, edgeXCheckDistance, whatIsEdge) ||
                Physics2D.Raycast(yCheck.position, Vector2.up, edgeYCheckDistance, whatIsEdge);
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (xCheck != null && yCheck != null)
        {
            Gizmos.DrawLine(xCheck.position, xCheck.position + new Vector3(edgeXCheckDistance, 0));
            Gizmos.DrawLine(yCheck.position, yCheck.position + new Vector3(0, edgeYCheckDistance));
        }

    }


}
