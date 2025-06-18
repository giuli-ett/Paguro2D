using System.Collections;
using UnityEngine;

public class Scintille : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private GameObject sparklesParent; 
    [SerializeField] private float sparkleDuration = 0.5f; 

    private bool hasSparkled = false;

    private void Awake()
    {
        if (sparklesParent != null)
        {
            sparklesParent.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Sparkles Parent non assegnato per la roccia: " + gameObject.name);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (sparklesParent != null && !hasSparkled)
            {
                StartCoroutine(ShowSparkles());
            }
        }
    }

    private IEnumerator ShowSparkles()
    {
        hasSparkled = true; // Imposta il flag per prevenire attivazioni multiple
        sparklesParent.SetActive(true); 

        // Attendi la durata specificata
        yield return new WaitForSeconds(sparkleDuration);

        sparklesParent.SetActive(false); // Disattiva il GameObject delle scintille
        hasSparkled = false; // Resetta il flag per permettere future scintille
    }
}
