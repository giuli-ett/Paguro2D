using System.Collections;
using UnityEngine;

public class ReteCutscene : MonoBehaviour
{
    [Header("Target della rete")]
    public Transform targetArrivo;
    public Transform targetUscita;

    [Header("Impostazioni movimento")]
    public float velocità = 3f;
    public float tempoAttesa = 5f;
    public float traslazioneRete = 5f;

    [Header("Animazioni")]
    public Animator animator;

    private bool inMovimento = false;
    private bool staUscendo = false;
    private bool haRaggiuntoArrivo = false;

    void Start()
    {
        StartCoroutine(IniziaCutscene());
    }

    IEnumerator IniziaCutscene()
    {
        yield return new WaitForSeconds(tempoAttesa);

        animator.SetBool("Pesca", true);
        inMovimento = true;
    }

    void Update()
    {
        if (!inMovimento) return;

        if (!staUscendo)
        {
            // Vai verso targetArrivo
            transform.position = Vector3.MoveTowards(transform.position, targetArrivo.position, velocità * Time.deltaTime);

            if (!haRaggiuntoArrivo && Vector3.Distance(transform.position, targetArrivo.position) < 0.05f)
            {
                haRaggiuntoArrivo = true;
                StartCoroutine(AttendiEPoiEsci());
            }
        }
        else
        {
            // Vai verso targetUscita
            transform.position = Vector3.MoveTowards(transform.position, targetUscita.position, velocità * Time.deltaTime);
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