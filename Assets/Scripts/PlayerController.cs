using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 6.0f;

    [SerializeField]
    private LayerMask groundLayer;

    private float groundedDistance = 0.02f;

    private PlayerInput playerInput;

    private Vector3 deltaMovement = new Vector3();
    private bool grounded = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        deltaMovement.x = playerInput.Horizontal * movementSpeed * Time.deltaTime;

        if (playerInput.Jumping)
        {
            grounded = false;
            deltaMovement.y = 10.0f * Time.deltaTime;
        }
        else if (!grounded)
        {
            deltaMovement.y = Physics2D.gravity.y * Time.deltaTime;
        }

        if (deltaMovement.y != 0)
        {
            MoveVertical(ref deltaMovement);
        }

        transform.Translate(deltaMovement);
    }

    void MoveVertical(ref Vector3 delta)
    {
        var goingUp = delta.y > 0;
        var distance = Mathf.Abs(delta.y);
        var direction = goingUp ? Vector2.up : -Vector2.up;

        Debug.DrawRay(transform.position, direction, Color.red);
        var hit = Physics2D.Raycast(transform.position, direction, distance, groundLayer);
        if (hit && !goingUp)
        {
            delta.y = goingUp ? hit.distance : -hit.distance;
            delta.y += groundedDistance;
        }

        grounded = hit && !goingUp;
    }
}
