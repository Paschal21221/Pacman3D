using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    public TMP_Text scoreText;
    public TMP_Text livesText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Refresh();
    }

    public static void Refresh()
    {
        if (Instance == null) return;
        if (GameManager.Instance == null) return;

        if (Instance.scoreText != null)
            Instance.scoreText.text = "Score: " + GameManager.Instance.score;

        if (Instance.livesText != null)
            Instance.livesText.text = "Lives: " + GameManager.Instance.lives;
    }
}