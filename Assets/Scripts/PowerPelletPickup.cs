using UnityEngine;

public class PowerPelletPickup : MonoBehaviour
{
    public float duration = 7f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.TriggerPowerMode();
        Destroy(gameObject);
    }
}