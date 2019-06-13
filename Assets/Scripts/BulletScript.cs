using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float BulletSpeed = 10f;

    public GameObject powPrefab;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        transform.position += transform.up * BulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && gameObject.layer == 12)
        {
            gameManager.OnPlayerHit(collision.gameObject, gameObject);
            Destroy(gameObject);
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
