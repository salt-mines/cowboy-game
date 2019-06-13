using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    internal GameManager gameManager;

    public void OnResume()
    {
        Destroy(gameObject);
        gameManager.Resume();
    }

    public void OnRestart()
    {
        Destroy(gameObject);
        gameManager.Restart();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
