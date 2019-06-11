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
    /// Minimum jump hold length in seconds.
    /// </summary>
    [Tooltip("Minimum jump hold length in seconds")]
    [SerializeField]
    private float jumpMinSustain = 0.05f;

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
    private bool isJumping;
    private bool hasStoppedHoldingJump;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController2D>();

        currentJumpGravity = jumpGravityStart;

        if (jumpMinSustain > jumpSustain)
        {
            jumpMinSustain = jumpSustain;
        }
    }

    void FixedUpdate()
    {
        var grounded = controller.Grounded;
        var holdingJump = playerInput.Jumping;

        deltaMovement.x = playerInput.Horizontal * movementSpeed;

        // Prevent continuing jump again if player lets go of the key
        if (!holdingJump && isJumping && spentJumping > jumpMinSustain)
        {
            spentJumping = jumpSustain;
        }

        if (holdingJump && grounded)
        {
            if (hasStoppedHoldingJump)
            {
                // Jumping from the ground
                deltaMovement.y = jumpSpeed;
                spentJumping = 0;
                isJumping = true;
                hasStoppedHoldingJump = false;
                currentJumpGravity = jumpGravityStart;
            }
        }
        else if ((isJumping && holdingJump && spentJumping < jumpSustain) || (isJumping && spentJumping < jumpMinSustain))
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
            if (isJumping)
            {
                deltaMovement.y -= currentJumpGravity * Time.deltaTime;
            }
            else
            {
                deltaMovement.y -= fallingGravity * Time.deltaTime;
            }
        }

        if (!holdingJump && grounded)
        {
            // Reset jump timer
            spentJumping = 0;
            isJumping = false;
        }

        controller.Move(deltaMovement * Time.deltaTime);

        if (!holdingJump && controller.Grounded)
        {
            hasStoppedHoldingJump = true;
        }

        if (controller.Grounded)
        {
            deltaMovement.y = 0;
        }
    }
}