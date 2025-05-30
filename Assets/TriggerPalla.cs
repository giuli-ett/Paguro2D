using UnityEngine;

public class TriggerPalla : MonoBehaviour
{
    public Palla palla;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            palla.StartRolling();
        }
    }
}
