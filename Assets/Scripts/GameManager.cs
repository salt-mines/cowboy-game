using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerLives = 3;

    public GameObject playerSpawnpoint;

    // Menus
    public Canvas canvas;
    public EventSystem eventSystem;

    public GameObject pauseMenuPrefab;
    public GameObject gameOverMenuPrefab;

    private bool isGameOver = false;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    void Awake()
    {
        var gm = GameObject.FindGameObjectsWithTag("GameController");

        if (gm.Length > 1)
        {
            Destroy(gameObject);
        }
    }

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
}
