using UnityEngine;

public class PowerPelletPickup : MonoBehaviour
{
    public int points = 50;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.AddScore(points);
        GameManager.Instance.PelletEaten();
        Destroy(gameObject);
    }
}
