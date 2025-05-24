using UnityEngine;

public class PesceCattivo : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    private Vector3 startPosition;

    [Header("VARIABILI")]
    [SerializeField] private float velocita = 2f;
    [SerializeField] private float distanza = 1.2f;

    void Start()
    {
        startPosition = transform.position;
    }

    private void Move()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * velocita) * distanza;
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);
    }
}
