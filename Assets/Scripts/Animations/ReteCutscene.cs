using System.Collections;
using UnityEngine;

public class ReteCutscene : MonoBehaviour
{
    [Header("Target della rete")]
    public Transform targetArrivo;
    public Transform targetUscita;

    [Header("Impostazioni movimento")]
    public float velocita = 3f;
    public float tempoAttesa = 5f;
    public float traslazioneRete = 5f;

    [Header("Animazioni")]
    public Animator animator;

    private bool inMovimento = false;
    private bool staUscendo = false;
    private bool haRaggiuntoArrivo = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false; // Nasconde la sprite all'avvio

        StartCoroutine(IniziaCutscene());
    }

    IEnumerator IniziaCutscene()
    {
        yield return new WaitForSeconds(tempoAttesa);

        if (spriteRenderer != null)
            spriteRenderer.enabled = true; // Mostra la sprite

        animator.SetBool("Pesca", true);
        inMovimento = true;
    }

    void Update()
    {
        if (!inMovimento) return;

        if (!staUscendo)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetArrivo.position, velocita * Time.deltaTime);

            if (!haRaggiuntoArrivo && Vector3.Distance(transform.position, targetArrivo.position) < 0.05f)
            {
                haRaggiuntoArrivo = true;
                StartCoroutine(AttendiEPoiEsci());
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetUscita.position, velocita * Time.deltaTime);
        }
    }

    IEnumerator AttendiEPoiEsci()
    {
        yield return new WaitForSeconds(1f);
        staUscendo = true;
    }

    public void TraslaDestra()
    {
        transform.position += Vector3.right * traslazioneRete;
    }
}