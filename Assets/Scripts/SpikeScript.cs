using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private GameManager gameManager;

    public Transform playerRespawnPosition;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameManager.OnPlayerHit(collision.gameObject, gameObject);

            if (playerRespawnPosition)
            {
                collision.gameObject.transform.position = playerRespawnPosition.position;
            }
        }
    }
}
