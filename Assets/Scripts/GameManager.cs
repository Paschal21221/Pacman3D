using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score { get; private set; }
    public int lives { get; private set; }

    int pelletsRemaining;

    [SerializeField] int startingLives = 3;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        lives = startingLives;
        score = 0;
        HUDController.Refresh();
    }

    public void RecountPellets()
    {
        int normal = FindObjectsByType<PelletPickup>(FindObjectsSortMode.None).Length;
        int power = FindObjectsByType<PowerPelletPickup>(FindObjectsSortMode.None).Length;
        pelletsRemaining = normal + power;
    }

    public void AddScore(int amount)
    {
        score += amount;
        HUDController.Refresh();
    }

    public void PelletEaten()
    {
        pelletsRemaining--;
        if (pelletsRemaining <= 0)
            Debug.Log("YOU WIN!");
    }

    public void LoseLife()
    {
        lives--;
        HUDController.Refresh();

        if (lives <= 0)
            Debug.Log("GAME OVER");
    }

    public bool IsGameOver()
    {
        return lives <= 0;
    }

    public void TriggerPowerMode()
    {
        foreach (GhostAI g in FindObjectsOfType<GhostAI>())
            g.mode = GhostMode.Frightened;

        CancelInvoke(nameof(EndPowerMode));
        Invoke(nameof(EndPowerMode), 7f);
    }

    void EndPowerMode()
    {
        foreach (GhostAI g in FindObjectsOfType<GhostAI>())
            g.mode = GhostMode.Normal;
    }
}