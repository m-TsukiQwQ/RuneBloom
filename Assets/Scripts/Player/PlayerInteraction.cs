using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform[] _targetCheck; 
    [SerializeField] private Vector2 _targetCheckRange;
    [SerializeField] private LayerMask _whatIsTarget;
    private PlayerMovement _playerMovement;
    [SerializeField] private Collider2D _check;


    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public virtual void Interact()
    {
        Collider2D detectedTarget = GetDetectedCollider();
        if (detectedTarget == null) return;

        IInteractable interactable = detectedTarget.GetComponentInParent<IInteractable>();

        if (interactable == null) return;


        interactable.Intercat();



    }


    private Collider2D GetDetectedCollider()
    {
        int index = GetDominantDirection();

        Collider2D target = Physics2D.OverlapBox(_targetCheck[index].position, _targetCheckRange, 0, _whatIsTarget);

        _check = target;
        return target;
    }

    private int GetDominantDirection() //up = 0, left = 1, down = 2, right = 3
    {
        Vector2 playerIdle = _playerMovement.idleInput;

        if (Mathf.Abs(playerIdle.x) > Mathf.Abs(playerIdle.y))
        {
            // --- Horizontal is dominant ---
            // If direction.x is positive, cast right. Otherwise, cast left.
            return (playerIdle.x > 0) ? 3 : 1;
        }
        else
        {
            // --- Vertical is dominant ---
            // If direction.y is positive, cast up. Otherwise, cast down.
            return (playerIdle.y > 0) ? 2 : 0;
        }


    }

}
