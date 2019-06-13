using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombRevolverShoot : MonoBehaviour
{
    public GameObject barrel;
    public GameObject bulletPrefab;

    public float shootDelay = 3f;
    private float timeElapsed;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > shootDelay)
        {
            Shoot();
            timeElapsed = 0;
        }
    }

    void Shoot()
    {
        GameObject firedBullet = Instantiate(bulletPrefab, barrel.transform.position, barrel.transform.rotation);
        firedBullet.layer = 12;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.OnPlayerHit(collision.gameObject, gameObject);
        }
    }
}
