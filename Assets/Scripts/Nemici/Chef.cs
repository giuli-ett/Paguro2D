using System.Collections;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [Header("Target da inseguire")]
    private Transform target;

    [Header("Parametri movimento")]
    [SerializeField] private float velocitaMovimento = 3f;
    [SerializeField] private float distanzaStop = 1f;
    [SerializeField] private float velocitaPresa = 5f;
    [SerializeField] private float durataPresa = 2f;
    [SerializeField] private float tempoMassimoAttivo = 5f;

    private Player player;
    private Rigidbody2D playerRb;

    private bool isGrabbing = false;
    private bool isActive = false;
    private float activeTimer = 0f;
    private float grabTimer = 0f;

    private Vector3 posizioneIniziale;

    void Start()
    {
        posizioneIniziale = transform.position;
        gameObject.SetActive(false); // parte disattivato
    }

    void Update()
    {
        if (!isActive) return;

        activeTimer += Time.deltaTime;

        if (isGrabbing)
        {
            grabTimer += Time.deltaTime;
            transform.position += Vector3.up * velocitaPresa * Time.deltaTime;

            if (grabTimer >= durataPresa)
            {
                isGrabbing = false;
                grabTimer = 0f;

                if (target != null) target.SetParent(null);
                if (playerRb != null)
                {
                    playerRb.isKinematic = false;
                    playerRb.simulated = true;
                }
                if (player != null) player.enabled = true;

                LifeController vita = target.GetComponent<LifeController>();
                if (vita != null)
                {
                    vita.Die();
                }

                RitornaSu();
            }

            return;
        }

        if (player == null || player.isInvisible) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance > distanzaStop)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position += (Vector3)(direction * velocitaMovimento * Time.deltaTime);
        }

        if (activeTimer >= tempoMassimoAttivo)
        {
            RitornaSu(); // si ritira se non ha preso nulla
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGrabbing || !isActive) return;

        if (other.CompareTag("Player"))
        {
            player.enabled = false;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.isKinematic = true;
            playerRb.simulated = false;

            target.SetParent(transform);

            isGrabbing = true;
            grabTimer = 0f;
        }
    }

    public void Attiva(Transform nuovoTarget)
    {
        gameObject.SetActive(true);
        transform.position = posizioneIniziale;
        isActive = true;
        activeTimer = 0f;
        target = nuovoTarget;

        player = target.GetComponent<Player>();
        playerRb = target.GetComponent<Rigidbody2D>();

        if (player == null || playerRb == null)
        {
            Debug.LogError("Chef: Player o Rigidbody2D non trovati!");
        }
    }

    private void RitornaSu()
    {
        isActive = false;
        StartCoroutine(RisalitaEFine());
    }

    private IEnumerator RisalitaEFine()
    {
        float altezzaFinale = transform.position.y + 5f;
        while (transform.position.y < altezzaFinale)
        {
            transform.position += Vector3.up * velocitaPresa * Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanzaStop);
    }
}