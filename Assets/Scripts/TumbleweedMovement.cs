using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedMovement : MonoBehaviour
{
    public Collider2D enemyPlatform;
    private Collider2D enemyCollider;
    private SpriteRenderer spriteRenderer;

    public float speed = 4f;

    private Vector3 currentPos;

    private Vector2 platformLeftCorner;
    private Vector2 platformRightCorner;

    private bool enemyAtLeftCorner = false;

    private GameManager gameManager;

    public GameObject oofPrefab;

    // Start is called before the first frame update
    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        platformLeftCorner = new Vector2(enemyPlatform.bounds.min.x + enemyCollider.bounds.extents.x, enemyPlatform.bounds.max.y);
        platformRightCorner = new Vector2(enemyPlatform.bounds.max.x - enemyCollider.bounds.extents.x, enemyPlatform.bounds.max.y);

        transform.position = platformRightCorner;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDirection();

        if (enemyAtLeftCorner)
        {
            currentPos = Vector3.MoveTowards(transform.position, platformRightCorner, speed * Time.deltaTime);
        }
        else
        {
            currentPos = Vector3.MoveTowards(transform.position, platformLeftCorner, speed * Time.deltaTime);
        }

        transform.position = currentPos;
    }

    void CheckDirection()
    {
        if (transform.position.x.Equals(platformLeftCorner.x) && !enemyAtLeftCorner)
        {
            spriteRenderer.flipX = true;
            enemyAtLeftCorner = true;
        }
        if (transform.position.x.Equals(platformRightCorner.x) && enemyAtLeftCorner)
        {
            spriteRenderer.flipX = false;
            enemyAtLeftCorner = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.playerLives--;
            Destroy(collision.gameObject);
            Instantiate(oofPrefab, transform.position, transform.rotation);
        }
    }
}

