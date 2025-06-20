using System.Collections;
using UnityEngine;

public class GuscioCutscene : MonoBehaviour
{
    [Header("Impostazioni temporali")]
    public float tempoIniziale = 2f; // Tempo prima della prima rotazione
    public float tempoRipristino = 7f; // Tempo dopo la prima rotazione per tornare indietro

    [Header("Angoli di rotazione")]
    public float angoloRotazione = -13f;

    private Quaternion rotazioneOriginale;

    void Start()
    {
        rotazioneOriginale = transform.rotation;
        StartCoroutine(GestisciRotazione());
    }

    private IEnumerator GestisciRotazione()
    {
        // Aspetta il primo tempo
        yield return new WaitForSeconds(tempoIniziale);

        // Applica la rotazione iniziale su Z
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + angoloRotazione);

        // Aspetta il secondo tempo
        yield return new WaitForSeconds(tempoRipristino);

        // Ruota di +13 (oppure annulla la rotazione)
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - angoloRotazione);
    }
}
