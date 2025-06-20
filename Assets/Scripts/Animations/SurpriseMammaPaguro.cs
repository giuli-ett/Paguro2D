using System.Collections;
using UnityEngine;

public class SpriteBlinkTimer : MonoBehaviour
{
    public float tempoPrimaApparizione = 10f; // Tempo in secondi prima che la sprite appaia
    public float tempoSorpresa = 3f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Nascondi la sprite all'inizio
            StartCoroutine(GestisciApparizione());
        }
        else
        {
            Debug.LogWarning("SpriteRenderer non trovato su " + gameObject.name);
        }
    }

    private IEnumerator GestisciApparizione()
    {
        yield return new WaitForSeconds(tempoPrimaApparizione);

        spriteRenderer.enabled = true; // Mostra la sprite

        yield return new WaitForSeconds(tempoSorpresa); // Aspetta 2 secondi

        spriteRenderer.enabled = false; // Nascondi di nuovo la sprite
    }
}
