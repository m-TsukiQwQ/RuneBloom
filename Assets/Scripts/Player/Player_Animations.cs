using UnityEngine;

public class Player_Animations : MonoBehaviour
{
    private Player _player;
    private Animator _animator;


    private readonly int _moveX = Animator.StringToHash("MoveX");
    private readonly int _moveY = Animator.StringToHash("MoveY");

    private readonly int _idleX = Animator.StringToHash("LastInputX");
    private readonly int _idleY = Animator.StringToHash("LastInputY");

    private readonly int _attackX = Animator.StringToHash("AttackX");
    private readonly int _attackY = Animator.StringToHash("AttackY");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponentInParent<Player>();
    }
    public void SetMoveAnimation(Vector2 direction)
    {
        _animator.SetFloat(_moveX, direction.x);
        _animator.SetFloat(_moveY, direction.y);

    }
    public void SetIdleAnimation(Vector2 direction)
    {
        Debug.Log(direction + " from anim");
        _animator.SetFloat(_idleX, direction.x);
        _animator.SetFloat(_idleY, direction.y);

    }

    public void SetAttackAnimation(Vector2 direction)
    {
        Debug.Log(direction + " from attack");
        _animator.SetFloat(_attackX, direction.x);
        _animator.SetFloat(_attackY, direction.y);

    }

    public void CurrentStateTrigger()
    {
        _player.CallAnimationTrigger();
    }


}
