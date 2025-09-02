using UnityEngine;
using System.Collections;

public class Fantasma : MonoBehaviour
{
    [Header("Target da inseguire")]
    private Transform target;

    [Header("Parametri movimento")]
    [SerializeField] private float velocitaMovimento = 2f;
    [SerializeField] private float distanzaStop = 0.5f;
    [SerializeField] private float tempoAttivo = 15f;
    [SerializeField] private float tempoFermoDopoColpo = 1f;

    private Player player;
    private bool isActive = false;
    private bool isCoolingDown = false;
    private float activeTimer = 0f;
    private Vector3 posizioneIniziale;

    // Animazione
    private Animator animator;
    private bool lastIsStop = false;

    void Start()
    {
        posizioneIniziale = transform.position;
        gameObject.SetActive(false); // parte disattivato
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isActive || isCoolingDown || player == null || player.isInvisible)
        {
            SetAnimazioneStop(true);
            return;
        }

        activeTimer += Time.deltaTime;

        if (activeTimer >= tempoAttivo)
        {
            gameObject.SetActive(false);
            return;
        }

        // Controlla se il giocatore è girato di spalle rispetto al fantasma
        bool playerFacingRight = player.isFacingRight;
        bool ghostIsOnRight = transform.position.x > target.position.x;
        bool playerIsFacingGhost = (playerFacingRight && ghostIsOnRight) || (!playerFacingRight && !ghostIsOnRight);

        float distance = Vector2.Distance(transform.position, target.position);
        bool shouldStop = distance <= distanzaStop || playerIsFacingGhost;

        SetAnimazioneStop(shouldStop);

        if (!shouldStop)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position += (Vector3)(direction * velocitaMovimento * Time.deltaTime);
        }
    }

    private void SetAnimazioneStop(bool value)
    {
        if (value != lastIsStop)
        {
            animator.SetBool("isStop", value);
            lastIsStop = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive || isCoolingDown) return;

        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayFantasma();
            StartCoroutine(PausaDopoColpo());
        }
    }

    private IEnumerator PausaDopoColpo()
    {
        isCoolingDown = true;
        SetAnimazioneStop(true);
        yield return new WaitForSeconds(tempoFermoDopoColpo);
        isCoolingDown = false;
    }

    public void Attiva(Transform nuovoTarget)
    {
        gameObject.SetActive(true);
        transform.position = posizioneIniziale;
        isActive = true;
        activeTimer = 0f;
        target = nuovoTarget;

        player = target.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Ghost: Player non trovato!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanzaStop);
    }
}
