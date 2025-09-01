using UnityEngine;

public class TriggerOssigenoInizio : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Ossigeno.Instance.StartOxygenConsumption();
        }
    }
}
