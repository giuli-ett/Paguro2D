using UnityEngine;

public class Medusa : MonoBehaviour
{
    [SerializeField] private float velocita = 2f;          
    [SerializeField] private float altezza = 1.2f;
     

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // Salva la posizione iniziale
        
    }

    void Update()
    {
        // Calcola la nuova posizione della medusa
        Move();

    }

    private void Move()
    {
        // Calcola la nuova posizione della medusa
        float newY = startPos.y + Mathf.Sin(Time.time * velocita) * altezza;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

}