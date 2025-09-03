using UnityEngine;

public class TriggerFantasma : MonoBehaviour
{
    [SerializeField] private Fantasma fantasma;

    private bool attivato = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attivato) return;

        if (other.CompareTag("Player"))
        {
            attivato = true;
            fantasma.Attiva(other.transform);
        }
    }
    public void ResetTrigger()
    {
        attivato = false;
    }

}