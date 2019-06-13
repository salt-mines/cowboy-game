using UnityEngine;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    internal GameManager gameManager;

    public TextMeshProUGUI textObject;

    public string loseText;
    public Color loseColor;

    public string winText;
    public Color winColor;

    public void Won()
    {
        textObject.text = winText;
        textObject.color = winColor;
    }

    public void Lost()
    {
        textObject.text = loseText;
        textObject.color = loseColor;
    }

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
