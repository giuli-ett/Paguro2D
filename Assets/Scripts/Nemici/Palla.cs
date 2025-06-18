using System.Collections;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class Palla : MonoBehaviour
{
    [SerializeField] public float speed = 15f;
    [SerializeField] private float TempoVita = 5f;
    [SerializeField] private Transform startPoint;

    // --- NUOVO FLAG PER LA DIREZIONE ---
    [Header("Direzione Rotazione")]
    [Tooltip("Se spuntato, la palla rotoler� verso destra. Altrimenti, rotoler� verso sinistra.")]
    public bool rollRight = false; // Nuovo flag pubblico

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private Coroutine lifetimeCoroutine;
    private bool isRolling = false;
    public bool flag = false; // Il tuo flag esistente, il nuovo 'rollRight' � per la direzione

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // startPosition non � usata, quindi potresti rimuoverla o usarla per qualcosa
        transform.position = startPoint.position;
    }

    public void StartRolling()
    {
        flag = true; // Questo flag sembra essere per indicare che la palla � in movimento
        isRolling = true;
        gameObject.SetActive(true); // Assicurati che sia attiva prima di iniziare a muoversi

        // --- APPLICA LA DIREZIONE IN BASE AL FLAG ---
        float direction = rollRight ? 1f : -1f; // Se rollRight � true, direction � 1 (destra), altrimenti -1 (sinistra)
        rb.linearVelocity = new Vector2(speed * direction, 0f); // Applica la velocit� con la direzione scelta

        if (lifetimeCoroutine != null)
            StopCoroutine(lifetimeCoroutine);

        lifetimeCoroutine = StartCoroutine(LifetimeTimer());
    }


    private IEnumerator LifetimeTimer()
    {
        yield return new WaitForSeconds(TempoVita);
        gameObject.SetActive(false);
    }


    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
            lifetimeCoroutine = null;
        }

        isRolling = false;
        flag = false; // Assicurati di resettare anche questo flag se indica lo stato di "rotolamento"

        gameObject.SetActive(false); // disattiva prima
        transform.position = startPoint.position; // sposta dopo
        gameObject.SetActive(true); // riattiva alla fine
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LifeController vita = other.GetComponent<LifeController>();
            if (vita != null)
            {
                Debug.Log("Palla ha ucciso il giocatore!");
                vita.Die();
            }
        }
    }
}
