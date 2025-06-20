using System.Collections;
using UnityEngine;

public class MammaPaguroCutscene : MonoBehaviour
{
    public Transform puntoDestinazione;
    public float velocit‡ = 2f;
    public Animator animator;
    public Transform rete;
    public float attesa = 2f;

    private bool haRaggiuntoDestinazione = false;
    private bool ËCatturata = false;

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
        transform.position += direzione * velocit‡ * Time.deltaTime;

        if (Vector3.Distance(transform.position, puntoDestinazione.position) < 0.1f)
        {
            haRaggiuntoDestinazione = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);

            // Avvia la coroutine per passare a Pick dopo 1 secondo
            StartCoroutine(AttivaAnimazionePick());
        }
    }

    private IEnumerator AttivaAnimazionePick()
    {
        yield return new WaitForSeconds(attesa);

        animator.SetBool("Idle", false);
        animator.SetTrigger("Pick");
        Cattura();
    }

    public void Cattura()
    {
        if (!ËCatturata)
        {
            ËCatturata = true;
            transform.SetParent(rete);
        }
    }
}
