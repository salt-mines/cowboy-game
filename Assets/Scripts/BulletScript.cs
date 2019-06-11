using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float BulletSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.position += transform.up * BulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Debug.Log("Player died!");
        }
        if (collision.gameObject.tag == "Enemy" && gameObject.layer == 11)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Debug.Log("Enemy killed!");
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
