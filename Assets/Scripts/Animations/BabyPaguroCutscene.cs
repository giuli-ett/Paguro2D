using System.Collections;
using UnityEngine;

public class BabyPaguroCutscene : MonoBehaviour
{
    public Transform puntoDestinazione;
    public Transform puntoSecondario;
    public float velocita = 2f;
    private float velocitaOriginale; 

    public Animator animator;

    [Header("Tempi configurabili")]
    public float attesaPrimaDiHide = 2f;
    public float tempoNascosto = 2f;

    private bool haRaggiuntoDestinazione = false;
    private bool inSecondoMovimento = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        velocitaOriginale = velocita; 
        animator.SetBool("Walk", true);
    }

    void Update()
    {
        if (!haRaggiuntoDestinazione)
        {
            CamminaVerso(puntoDestinazione, ref haRaggiuntoDestinazione, () =>
            {
                animator.SetBool("Walk", false);
                StartCoroutine(PassaAHideDopoAttesa());
            });
        }
        else if (inSecondoMovimento)
        {
            CamminaVerso(puntoSecondario, ref inSecondoMovimento, () =>
            {
                animator.SetBool("Walk", false);
            });
        }
    }

    void CamminaVerso(Transform target, ref bool flagFine, System.Action onArrivo)
    {
        Vector3 direzione = (target.position - transform.position).normalized;
        transform.position += direzione * velocita * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            flagFine = true;
            onArrivo?.Invoke();
        }
    }

    private IEnumerator PassaAHideDopoAttesa()
    {
        yield return new WaitForSeconds(attesaPrimaDiHide);

        animator.SetBool("Hide", true);

        yield return new WaitForSeconds(tempoNascosto);

        animator.SetBool("Hide", false);
        animator.SetBool("DeHide", true);

        yield return new WaitForSeconds(1f);

        // Raddoppia la velocità ora
        velocita = velocitaOriginale * 2f;
        animator.SetBool("Walk", true);
        inSecondoMovimento = true;
    }
}
