using UnityEngine;

public class TriggerOssigenoFine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Ossigeno.Instance.StopOxygenConsumption();
        }
    }
}

