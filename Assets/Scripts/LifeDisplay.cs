using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    public void SetLives(int lives)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var c = transform.GetChild(i);

            if (lives > i)
                c.GetComponent<Image>().color = Color.white;
            else
                c.GetComponent<Image>().color = Color.black;
        }
    }
}
