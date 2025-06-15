using UnityEngine;

public class TriggerManoChef : MonoBehaviour
{
    [SerializeField] private Chef chef;

    private bool attivato = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attivato) return;

        if (other.CompareTag("Player"))
        {
            attivato = true;
            chef.Attiva(other.transform);
        }
    }
}
