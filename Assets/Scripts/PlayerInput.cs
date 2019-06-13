using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public bool Jumping { get; private set; }
    public bool Down { get; private set; }

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Jumping = Input.GetAxisRaw("Jump") > 0;
        Down = Input.GetAxisRaw("Vertical") < 0;

        if (Input.GetButton("Pause"))
        {
            gameManager.OpenPauseMenu();
        }
    }
}
