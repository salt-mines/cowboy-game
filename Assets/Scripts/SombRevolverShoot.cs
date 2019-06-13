using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SombRevolverShoot : MonoBehaviour
{
    public GameObject barrel;
    public GameObject bulletPrefab;

    public float shootDelay = 3f;
    private float timeElapsed;

    private bool onScreen = false;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        onScreen = GetComponent<Renderer>().isVisible;
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
        if (!onScreen) return;

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

    private void OnBecameVisible()
    {
        onScreen = true;
    }

    private void OnBecameInvisible()
    {
        onScreen = false;
    }
}
