﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Level system
    public string[] levels;
    private int currentLevel = 0;

    // Player health
    public float playerHitImmunityTime = 1f;
    public int playerLives = 3;
    private int currentLives;

    private float playerLastHit;

    public GameObject playerHurtPrefab;

    // Menus
    public Canvas canvas;
    public EventSystem eventSystem;

    public GameObject pauseMenuPrefab;
    public GameObject levelEndMenuPrefab;
    public GameObject gameOverMenuPrefab;

    private bool isGameOver = false;
    private bool paused = false;

    void Start()
    {
        Restart();
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

    public void OnPlayerHit(GameObject player, GameObject attacker)
    {
        if (Time.time - playerLastHit <= playerHitImmunityTime)
            return;

        playerLastHit = Time.time;
        currentLives--;

        if (playerHurtPrefab)
        {
            Instantiate(playerHurtPrefab, player.transform.position, Quaternion.identity);
        }

        GameObject.Find("Lives").GetComponent<LifeDisplay>().SetLives(currentLives);

        var sounds = player.GetComponent<PlayerSounds>();
        if (currentLives == 0)
        {
            if (sounds)
                sounds.PlayDeath();

            GameOver(false);
        }
        else
        {
            if (sounds)
                sounds.PlayHurt();
        }
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
        paused = false;
        isGameOver = false;
        Time.timeScale = 1;
        currentLevel = 0;
        currentLives = playerLives;

        GameObject.Find("Lives").GetComponent<LifeDisplay>().SetLives(currentLives);

        UnloadLevel();

        SceneManager.LoadSceneAsync(levels[0], LoadSceneMode.Additive);
    }

    void UnloadLevel()
    {
        var count = SceneManager.sceneCount;
        if (count > 1)
        {
            for (int i = 0; i < count; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex != 0)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
        }
    }

    public void LoadNextLevel()
    {
        if (currentLevel >= levels.Length - 1)
            currentLevel = 0;
        else
            currentLevel += 1;

        UnloadLevel();

        SceneManager.LoadSceneAsync(levels[currentLevel], LoadSceneMode.Additive);

        Resume();
    }

    public void ReachedLevelEnd()
    {
        Pause();

        if (currentLevel >= levels.Length - 1)
            GameOver(true);
        else
            OpenLevelEndMenu();
    }

    void GameOver(bool won)
    {
        isGameOver = true;
        Pause();
        OpenGameOverMenu(won);
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

    public void OpenLevelEndMenu()
    {
        GameObject menu = Instantiate(levelEndMenuPrefab, canvas.gameObject.transform);
        var goMenu = menu.GetComponent<LevelEndMenu>();
        goMenu.gameManager = this;
        eventSystem.SetSelectedGameObject(menu.transform.GetChild(0).gameObject);
    }

    public void OpenGameOverMenu(bool won)
    {
        if (!isGameOver) return;

        GameObject menu = Instantiate(gameOverMenuPrefab, canvas.gameObject.transform);
        var goMenu = menu.GetComponent<GameOverMenu>();
        goMenu.gameManager = this;
        eventSystem.SetSelectedGameObject(menu.transform.GetChild(0).gameObject);
        if (won)
            goMenu.Won();
        else
            goMenu.Lost();
    }
    #endregion
}
