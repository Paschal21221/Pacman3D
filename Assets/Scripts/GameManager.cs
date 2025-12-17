using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    int pelletsRemaining = 0;

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
        // intentionally empty
    }

    // Call this after you spawn/build pellets
    public void RecountPellets()
    {
        int normal = FindObjectsByType<PelletPickup>(FindObjectsSortMode.None).Length;
        int power = FindObjectsByType<PowerPelletPickup>(FindObjectsSortMode.None).Length;

        pelletsRemaining = normal + power;
        Debug.Log("Pellets counted: " + pelletsRemaining);
    }


    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    public void PelletEaten()
    {
        pelletsRemaining--;
        if (pelletsRemaining <= 0)
        {
            Debug.Log("YOU WIN!");
        }
    }

    public void TriggerPowerMode()
    {
        Debug.Log("POWER MODE!");
        // We'll implement scared ghosts next
    }
    

}
