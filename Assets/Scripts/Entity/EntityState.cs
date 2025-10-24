using UnityEngine;

public abstract class EntityState
{
    //components
    protected Animator _animator;
    protected Rigidbody2D _rb2d;
    
    protected StateMachine _stateMachine;

    protected string _animBoolName;
    protected bool _triggerCalled;
    protected float _stateTimer;
    
    public EntityState( StateMachine stateMachine, string animBoolName)
    {
        
        this._stateMachine = stateMachine;
        this._animBoolName = animBoolName;

    }

    public virtual void Enter() //Every time state will be changed, enter will be called
    {
        _animator.SetBool(_animBoolName, true);
        //Debug.Log("Entering " +  _animBoolName);
        _triggerCalled = false;
    }


    public virtual void Update() // logic of the state
    {
        //Debug.Log("In " + _animBoolName);
        _stateTimer -= Time.deltaTime;

        
    }
    

    public virtual void Exit() // this will be called, everytime we exit state and change to a new one
    {
        //Debug.Log("Exiting " + _animBoolName);
        _animator.SetBool(_animBoolName, false);

    }

    public void CallAnimationTrigger()
    {
        _triggerCalled = true;
    }
}
