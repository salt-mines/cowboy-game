using UnityEngine;

public class LevelEndMenu : MonoBehaviour
{
    internal GameManager gameManager;

    public void OnNextLevel()
    {
        Destroy(gameObject);
        gameManager.LoadNextLevel();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
