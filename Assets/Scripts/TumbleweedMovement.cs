using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedMovement : MonoBehaviour
{
    public Collider2D enemyPlatform;
    private Collider2D enemyCollider;

    public float speed = 4f;

    private Vector3 currentPos;

    private Vector2 platformLeftCorner;
    private Vector2 platformRightCorner;

    

    // Start is called before the first frame update
    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();

        platformLeftCorner = new Vector2(enemyPlatform.bounds.min.x + enemyCollider.bounds.extents.x, enemyPlatform.bounds.max.y);
        platformRightCorner = new Vector2(enemyPlatform.bounds.max.x - enemyCollider.bounds.extents.x, enemyPlatform.bounds.max.y);

        transform.position = platformRightCorner;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = Vector3.MoveTowards(transform.position, platformLeftCorner, speed * Time.deltaTime);
        transform.position = currentPos;
    }
}
