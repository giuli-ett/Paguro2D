using System.Collections;
using UnityEngine;

public class BabyPaguroCutscene : MonoBehaviour
{
    public Transform puntoDestinazione;
    public float velocita = 2f;
    public Animator animator;
    public float attesa = 2f; // Tempo di attesa prima dell'animazione "Hide"

    private bool haRaggiuntoDestinazione = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetBool("Walk", true);
    }

    void Update()
    {
        if (!haRaggiuntoDestinazione)
        {
            CamminaVersoDestinazione();
        }
    }

    void CamminaVersoDestinazione()
    {
        Vector3 direzione = (puntoDestinazione.position - transform.position).normalized;
        transform.position += direzione * velocita * Time.deltaTime;

        if (Vector3.Distance(transform.position, puntoDestinazione.position) < 0.1f)
        {
            haRaggiuntoDestinazione = true;
            animator.SetBool("Walk", false);

            // Avvia la coroutine che attende e poi attiva Hide
            StartCoroutine(PassaAHideDopoAttesa());
        }
    }

    private IEnumerator PassaAHideDopoAttesa()
    {
        yield return new WaitForSeconds(attesa);
        animator.SetBool("Hide", true);
    }
}
