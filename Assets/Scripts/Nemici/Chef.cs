using UnityEngine;

public class Chef : MonoBehaviour
{
    [Header("Target da inseguire")]
    public Transform target;

    [Header("Parametri movimento")]
    [SerializeField] private float velocitàMovimento = 3f;
    [SerializeField] private float distanzaStop = 1f;
    [SerializeField] private float velocitàPresa = 5f;
    [SerializeField] private float durataPresa = 2f;

    private Player player;
    private Rigidbody2D playerRb;

    private bool isGrabbing = false;
    private float grabTimer = 0f;

    void Start()
    {
        if (target != null)
        {
            player = target.GetComponent<Player>();
            playerRb = target.GetComponent<Rigidbody2D>();
        }

        if (player == null || playerRb == null)
            Debug.LogError("Chef: Player o Rigidbody2D non trovati!");
    }

    void Update()
    {
        
        if (isGrabbing)
        {
            grabTimer += Time.deltaTime;

            // Muove la mano verso l’alto
            transform.position += Vector3.up * velocitàPresa * Time.deltaTime;

            if (grabTimer >= durataPresa)
            {
                isGrabbing = false;
                grabTimer = 0f;

                Debug.Log("Il paguro è stato catturato!");

                // Scollega il paguro dalla mano
                if (target != null) target.SetParent(null);

                // Riattiva Rigidbody
                if (playerRb != null)
                {
                    playerRb.isKinematic = false;
                    playerRb.simulated = true;
                }

                // Riattiva controlli del player
                if (player != null) player.enabled = true;

                // Chiama il metodo di morte dal LifeController
                LifeController vita = target.GetComponent<LifeController>();
                if (vita != null)
                {
                    vita.Die();
                }
            }
        }

        if (player == null || player.isInvisible) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance > distanzaStop)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position += (Vector3)(direction * velocitàMovimento * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGrabbing) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Mano ha afferrato il paguro!");

            // Blocca fisica e controlli del player
            if (player != null)
            {
                player.enabled = false;
            }

            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.isKinematic = true;
                playerRb.simulated = false;
            }

            // Attacca il player alla mano
            target.SetParent(this.transform);

            isGrabbing = true;
            grabTimer = 0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanzaStop);
    }
}