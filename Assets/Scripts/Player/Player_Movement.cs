using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _runSpeedMultiplier = 1;

    public float walkSpeed => _walkSpeed;
    public float runSpeedMultiplier => _runSpeedMultiplier;


    public PlayerInputSystem input { get; private set; }
    public Rigidbody2D RigidBody2D { get; private set; }

    public Vector2 moveInput {  get; private set; }
    public Vector2 idleInput {  get; private set; }

    private Player _player;

    private void Awake()
    {
        RigidBody2D = GetComponent<Rigidbody2D>();
        input = new PlayerInputSystem();
        _player = GetComponent<Player>();
    }
    public Vector2 IdleDirection()
    {
        return idleInput = input.Player.Movement.ReadValue<Vector2>().normalized;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        RigidBody2D.linearVelocity = new Vector2(xVelocity, yVelocity);
    }
  
    
    private void OnEnable()
    {
        
        // started - input just begun(press key down), performed - input is in process (hold the key), canceled - input stops (release the key)
        input.Enable();

        
        input.Player.Movement.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();

            // Only update idleInput if the moveInput is not zero, ensuring we store the direction
            // and not an accidental (0,0) reading. We normalize it to get a pure direction vector.
            if (moveInput != Vector2.zero)
            {
              
                
                idleInput = moveInput;

            }
        };

        
        input.Player.Movement.canceled += ctx =>
        {
            moveInput = Vector2.zero;
            
        };

        input.Player.Attack.performed += ctx =>
        {
            idleInput = _player.inputHandler.mouseDirection;//mouse position (-1..1, -1..1)
        };

    }

    private void OnDisable()
    {
        input.Disable();
    }

}
