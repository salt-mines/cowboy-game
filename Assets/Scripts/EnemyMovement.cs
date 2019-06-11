using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    public Collider2D enemyPlatform;
    
    public float enemySpeed = 3f;
    public float enemyJumpHeight = 5f;

    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;

    private Vector2 platformLeftCorner;
    private Vector2 platformRightCorner;

    private float sinThings;
    private float elapsedTime;
    private float moveWaitTime = 1f;

    private bool enemyAtLeftCorner = false;


    private void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        platformLeftCorner = new Vector2(enemyPlatform.bounds.min.x + enemyCollider.bounds.extents.x, enemyPlatform.bounds.max.y);
        platformRightCorner = new Vector2(enemyPlatform.bounds.max.x - enemyCollider.bounds.extents.x, enemyPlatform.bounds.max.y);

        transform.position = platformRightCorner;         
    }

    // Start is called before the first frame update
    void Start()
    {
    
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
        if(transform.position.x.Equals(platformLeftCorner.x) && !enemyAtLeftCorner)
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
            return;
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
    }
}
