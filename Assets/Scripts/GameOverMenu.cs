using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    internal GameManager gameManager;

    public void OnNewGame()
    {
        Destroy(gameObject);
        gameManager.Restart();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
