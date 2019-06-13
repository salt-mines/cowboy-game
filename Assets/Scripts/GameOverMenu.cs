using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void OnNewGame()
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
