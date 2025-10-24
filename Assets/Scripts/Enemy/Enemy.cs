using UnityEngine;

public class Enemy : Entity
{
    //states
    public Enemy_State_Idle idleState;
    public Enemy_State_Wander wanderState;


    [Header("General Movement Details")]
    public float idleTime = 2f;
    public float moveSpeed;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Wander State")]
    public float wanderTime;
    public Vector2 wanderRange;
    //[HideInInspector]
    public Vector3 wanderPosition;



    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, wanderRange);
        Gizmos.DrawLine(transform.position, wanderPosition);

    }
}
