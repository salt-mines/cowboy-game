﻿using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    public Collider2D enemyPlatform;

    public float enemySpeed = 3f;

    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;
    private Animator animator;

    private Vector2 platformLeftCorner;
    private Vector2 platformRightCorner;

    private float sinThings;
    private float elapsedTime;
    public float moveWaitTime = 2f;

    public bool startFromRight = true;

    private bool isJumping;
    private bool isPreJumping;

    private bool enemyAtLeftCorner = false;

    private GameManager gameManager;


    private void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        platformLeftCorner = new Vector2(enemyPlatform.bounds.min.x + enemyCollider.bounds.extents.x, enemyPlatform.bounds.max.y);
        platformRightCorner = new Vector2(enemyPlatform.bounds.max.x - enemyCollider.bounds.extents.x, enemyPlatform.bounds.max.y);

        transform.position = startFromRight ? platformRightCorner : platformLeftCorner;
        enemyAtLeftCorner = !startFromRight;
        spriteRenderer.flipX = !startFromRight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sinThings = (transform.position.x - platformLeftCorner.x) / (platformRightCorner.x - platformLeftCorner.x);

        CheckDirection();
        Move();
    }

    void CheckDirection()
    {
        if (transform.position.x.Equals(platformLeftCorner.x) && !enemyAtLeftCorner)
        {
            spriteRenderer.flipX = true;
            enemyAtLeftCorner = true;
            elapsedTime = 0;
        }
        if (transform.position.x.Equals(platformRightCorner.x) && enemyAtLeftCorner)
        {
            spriteRenderer.flipX = false;
            enemyAtLeftCorner = false;
            elapsedTime = 0;
        }
    }

    private Vector3 currentPos;

    void Move()
    {
        if (moveWaitTime > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            if(moveWaitTime-elapsedTime < 0.5f && !isPreJumping)
            {
                animator.SetTrigger("Prejump");
                isPreJumping = true;
            }
            return;
        }
        isPreJumping = false;
        if (!isJumping)
        { 
            animator.SetTrigger("Jump");
        }
        

        if (enemyAtLeftCorner)
        {
            currentPos = Vector3.MoveTowards(transform.position, platformRightCorner, enemySpeed * Time.deltaTime);
        }
        else
        {
            currentPos = Vector3.MoveTowards(transform.position, platformLeftCorner, enemySpeed * Time.deltaTime);
        }
        currentPos.y = platformLeftCorner.y + Mathf.Sin(sinThings * Mathf.PI);
        transform.position = currentPos;

        if (currentPos.x == platformLeftCorner.x || currentPos.x == platformRightCorner.x)
        {
            isJumping = false;
            animator.SetTrigger("Land");
        }
        else
        {
            isJumping = true;
            animator.ResetTrigger("Land");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.OnPlayerHit(collision.gameObject, gameObject);
        }
    }
}
