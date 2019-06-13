using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaloonGoal : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("SaloonGoal Paused!");
            gameManager.OpenPauseMenu();
        }
    }
}
