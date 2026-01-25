using UnityEngine;

public class PelletPickup : MonoBehaviour
{
    public int points = 10;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.AddScore(points);
        GameManager.Instance.PelletEaten();
        Destroy(gameObject);
    }
}