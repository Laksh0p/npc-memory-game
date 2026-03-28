using UnityEngine;

public class GameOverText : MonoBehaviour
{
    public GameObject text;
    bool shown = false;

    public void Show()
    {
        if (shown) return;

        text.SetActive(true);
        shown = true;
    }
}