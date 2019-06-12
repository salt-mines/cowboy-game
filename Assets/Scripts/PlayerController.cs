using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    [Range(0.1f, 50f)]
    [SerializeField]
    private float movementSpeed = 6.0f;

    [Range(0f, 10f)]
    [SerializeField]
    private float accelerationTime = 0.2f;

    [Range(0f, 10f)]
    [SerializeField]
    private float decelerationTime = 0.1f;

    [Range(0f, 10f)]
    [SerializeField]
    private float directionChangeTime = 0.1f;

    [Range(0.01f, 10f)]
    [SerializeField]
    private float airControlAccelMultiplier = 0.8f;

    [Range(0.01f, 10f)]
    [SerializeField]
    private float airControlDecelMultiplier = 0.1f;

    [Range(0.01f, 10f)]
    [SerializeField]
    private float airDirectionChangeMultiplier = 0.8f;

    [Range(0.1f, 50f)]
    [SerializeField]
    private float jumpSpeed = 6.0f;

    /// <summary>
    /// How long holding the jump key increases jump height.
    /// </summary>
    [Tooltip("How long holding the jump key increases jump height")]
    [Range(0f, 2f)]
    [SerializeField]
    private float jumpSustain = 0.4f;

    /// <summary>
    /// Minimum jump hold length in seconds.
    /// </summary>
    [Tooltip("Minimum jump hold length in seconds")]
    [Range(0f, 2f)]
    [SerializeField]
    private float jumpMinSustain = 0.05f;

    /// <summary>
    /// Gravity for falling without jumping.
    /// </summary>
    [Tooltip("Gravity for falling without jumping")]
    [Range(0f, 1000f)]
    [SerializeField]
    private float fallingGravity = 10f;

    /// <summary>
    /// Gravity for low jumping.
    /// </summary>
    [Tooltip("Gravity for low jumping")]
    [Range(0f, 1000f)]
    [SerializeField]
    private float jumpGravityStart = 10f;

    /// <summary>
    /// Gravity for high jumping.
    /// </summary>
    [Tooltip("Gravity for high jumping")]
    [Range(0f, 1000f)]
    [SerializeField]
    private float jumpGravityEnd = 10f;

    [SerializeField]
    private bool lowerToGroundOnAwake = true;

    private float currentJumpGravity;

    private PlayerInput playerInput;
    private CharacterController2D controller;
    private Animator animator;

    private Vector3 deltaMovement;

    private float currentHorizontalMovement;
    private float currentAcceleration;

    private float spentJumping;
    private bool isJumping;
    private bool hasStoppedHoldingJump;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();

        currentJumpGravity = jumpGravityStart;

        if (jumpMinSustain > jumpSustain)
        {
            jumpMinSustain = jumpSustain;
        }

        if (lowerToGroundOnAwake)
            controller.LowerToGround();
    }

    void FixedUpdate()
    {
        var grounded = controller.Grounded;
        var holdingJump = playerInput.Jumping;
        var targetMovement = playerInput.Horizontal;

        var speed = accelerationTime;
        if (!grounded)
            speed *= 1 / airControlAccelMultiplier;

        if (Mathf.Abs(targetMovement) < Mathf.Abs(currentHorizontalMovement))
        {
            speed = decelerationTime;
            if (!grounded)
                speed *= 1 / airControlDecelMultiplier;
        }

        if (!Utils.SameSign(currentHorizontalMovement, targetMovement) && targetMovement != 0f)
        {
            speed = directionChangeTime;
            if (!grounded)
                speed *= 1 / airDirectionChangeMultiplier;
        }

        currentHorizontalMovement = Mathf.SmoothDamp(currentHorizontalMovement, targetMovement, ref currentAcceleration, speed);
        deltaMovement.x = currentHorizontalMovement * movementSpeed;

        if (animator)
            animator.SetFloat("Speed", Mathf.Abs(currentHorizontalMovement));

        // Prevent continuing jump again if player lets go of the key
        if (!holdingJump && isJumping && spentJumping > jumpMinSustain)
        {
            spentJumping = jumpSustain;
        }


        if (playerInput.Down && holdingJump && grounded)
        {
            controller.ignoreOneWayThisFrame = true;
        }
        else if (holdingJump && grounded)
        {
            if (hasStoppedHoldingJump)
            {
                // Jumping from the ground
                deltaMovement.y = jumpSpeed;
                spentJumping = 0;
                isJumping = true;
                hasStoppedHoldingJump = false;
                currentJumpGravity = jumpGravityStart;

                if (animator)
                {
                    animator.SetTrigger("Jump");
                    animator.SetBool("IsJumping", true);
                }
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

        controller.Move(deltaMovement * Time.deltaTime);

        if (isJumping && controller.Grounded)
        {
            // Reset jump timer
            spentJumping = 0;
            isJumping = false;
            if (animator)
            {
                animator.SetTrigger("Land");
                animator.SetBool("IsJumping", false);
            }
        }
        else
        {
            if (animator)
                animator.ResetTrigger("Land");
        }

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