using UnityEngine;

public class Murena : MonoBehaviour
{
    [SerializeField] private float velocit� = 2f;          
    [SerializeField] private float altezza = 1.2f;       

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // Salva la posizione iniziale
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * velocit�) * altezza;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
