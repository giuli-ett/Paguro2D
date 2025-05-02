using UnityEngine;

public class TentacoliMedusa : MonoBehaviour
{
    [SerializeField] private float velOscillazione = 2f;    
    [SerializeField] private float angOscillazione = 15f;   

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * velOscillazione) * angOscillazione;
        transform.localRotation = startRotation * Quaternion.Euler(0, 0, angle);
    }
}
