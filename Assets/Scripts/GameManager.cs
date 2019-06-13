using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerLives = 3;
    private int currentLives;

    public GameObject playerSpawnpoint;

    // Menus
    public Canvas canvas;
    public EventSystem eventSystem;

    public GameObject pauseMenuPrefab;
    public GameObject gameOverMenuPrefab;

    private bool isGameOver = false;
    private bool paused = false;

    void Start()
    {
        currentLives = playerLives;
    }

    void Awake()
    {
        var gm = GameObject.FindGameObjectsWithTag("GameController");

        if (gm.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 1;

        GameObject.Find("Lives").GetComponent<LifeDisplay>().SetLives(currentLives);
    }

    public void OnPlayerHit(GameObject player)
    {
        currentLives--;

        if (currentLives < 0)
        {

        }
        else
        {
            GameObject.Find("Lives").GetComponent<LifeDisplay>().SetLives(currentLives);
        }
    }

    void GameOver()
    {

    }

    #region UI Methods
    public void OpenPauseMenu()
    {
        if (paused) return;

        Pause();

        GameObject menu = Instantiate(pauseMenuPrefab, canvas.gameObject.transform);
        menu.GetComponent<PauseMenu>().gameManager = this;
        eventSystem.SetSelectedGameObject(menu.transform.GetChild(0).gameObject);
    }

    public void Pause()
    {
        if (paused) return;

        paused = true;

        Time.timeScale = 0;
    }

    public void Resume()
    {
        if (!paused) return;

        paused = false;

        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
