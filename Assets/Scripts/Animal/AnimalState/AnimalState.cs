using UnityEngine;

public class AnimalState : EntityState
{
    protected Animal _animal;
    public AnimalState(Animal animal, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this._animal = animal;
        _rb2d = animal.rb;
        _animator = animal.animator;
    }


}
