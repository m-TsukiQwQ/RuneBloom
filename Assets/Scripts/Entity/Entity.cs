using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{
    protected StateMachine _stateMachine;

    public Rigidbody2D rb;
    public Animator animator { get; private set; }

    [Header("Collision Detection")]
    [SerializeField] private float edgeXCheckDistance;
    [SerializeField] private float edgeYCheckDistance;
    [SerializeField] protected LayerMask whatIsEdge;
    [SerializeField] private Transform xCheck;
    [SerializeField] private Transform yCheck;
    public bool edgeDetected {  get; private set; }


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        _stateMachine = new StateMachine();

    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        _stateMachine.UpdateActiveState();
        HandleCollision();

    }

    public void CallAnimationTrigger()
    {
        _stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
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
