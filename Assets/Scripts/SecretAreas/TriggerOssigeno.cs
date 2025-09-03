using UnityEngine;

public class TriggerOssigeno : MonoBehaviour
{
    private bool isOxygenActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (isOxygenActive)
        {
            Ossigeno.Instance.StopOxygenConsumption();
        }
        else
        {
            Ossigeno.Instance.StartOxygenConsumption();
        }

        isOxygenActive = !isOxygenActive;
    }
}

