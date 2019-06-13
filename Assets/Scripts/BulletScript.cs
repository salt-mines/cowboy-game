using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float BulletSpeed = 10f;

    public GameObject powPrefab;

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
        
        if(collision.gameObject.tag == "Player" && gameObject.layer == 12)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject.transform);
        }
        if (collision.gameObject.tag == "Enemy" && gameObject.layer == 11)
        {
            onDeath(collision.gameObject.transform);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    private void onDeath(Transform transform)
    {
        Instantiate(powPrefab, transform.position, transform.rotation);

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
