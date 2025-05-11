using UnityEngine;

public class Tartaruga : MonoBehaviour
{
    [SerializeField] private float velocita = 2f;          
    [SerializeField] private float distanza = 1.2f;
     

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
    float newX = startPos.x + Mathf.Sin(Time.time * velocita) * distanza;
    transform.position = new Vector3(newX, startPos.y, startPos.z);
}


}
