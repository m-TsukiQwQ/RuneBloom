using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected StateMachine _stateMachine;

    public Animator animator { get; private set; }

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        _stateMachine = new StateMachine();

    }

    protected virtual void Start()
    {
        
    }

    private void Update()
    {
        _stateMachine.UpdateActiveState();

    }

    public void CallAnimationTrigger()
    {
        _stateMachine.currentState.CallAnimationTrigger();
    }

}
