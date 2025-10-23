using UnityEngine;

public abstract class EntityState
{
    
    protected StateMachine _stateMachine;
    protected string _animBoolName;
    protected bool _triggerCalled;
    protected Animator _animator;
    
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
