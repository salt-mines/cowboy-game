using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 6.0f;

    [SerializeField]
    private float jumpSpeed = 6.0f;

    /// <summary>
    /// How long holding the jump key increases jump height.
    /// </summary>
    [Tooltip("How long holding the jump key increases jump height")]
    [SerializeField]
    private float jumpSustain = 0.4f;

    /// <summary>
    /// Gravity for falling without jumping.
    /// </summary>
    [Tooltip("Gravity for falling without jumping")]
    [SerializeField]
    private float fallingGravity = 10f;

    /// <summary>
    /// Gravity for low jumping.
    /// </summary>
    [Tooltip("Gravity for low jumping")]
    [SerializeField]
    private float jumpGravityStart = 10f;

    /// <summary>
    /// Gravity for high jumping.
    /// </summary>
    [Tooltip("Gravity for high jumping")]
    [SerializeField]
    private float jumpGravityEnd = 10f;

    private float currentJumpGravity;

    private PlayerInput playerInput;
    private CharacterController2D controller;

    private Vector3 deltaMovement;

    private float spentJumping;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController2D>();

        currentJumpGravity = jumpGravityStart;
    }

    void FixedUpdate()
    {
        var grounded = controller.Grounded;
        var jumping = playerInput.Jumping;

        deltaMovement.x = playerInput.Horizontal * movementSpeed;

        // Prevent continuing jump again if player lets go of the key
        if (!jumping && spentJumping > 0)
        {
            spentJumping = jumpSustain;
        }

        if (jumping && grounded)
        {
            // Jumping from the ground
            deltaMovement.y = jumpSpeed;
            spentJumping = 0;
            currentJumpGravity = jumpGravityStart;
        }
        else if (jumping && spentJumping < jumpSustain)
        {
            // Continuing jump while jump key is held
            deltaMovement.y = jumpSpeed;
            spentJumping += Time.deltaTime;

            // If jump key is being held, scale jump gravity towards jumpGravityEnd.
            if (jumpSustain > 0)
            {
                currentJumpGravity = Mathf.Lerp(jumpGravityStart, jumpGravityEnd, spentJumping / jumpSustain);
            }
        }
        else
        {
            // Start falling
            if (spentJumping > 0)
            {
                deltaMovement.y -= currentJumpGravity * Time.deltaTime;
            }
            else
            {
                deltaMovement.y -= fallingGravity * Time.deltaTime;
            }
        }

        if (grounded)
        {
            // Reset jump timer
            spentJumping = 0;
        }

        controller.Move(deltaMovement * Time.deltaTime);

        if (grounded)
        {
            deltaMovement.y = 0;
        }
    }
}