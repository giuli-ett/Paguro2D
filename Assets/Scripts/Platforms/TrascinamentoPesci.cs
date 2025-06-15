/*using Unity.VisualScripting;
using UnityEngine;


public class TrascinamentoPesci : MonoBehaviour
{
    [SerializeField] private BancoPesci fishGroup;
    [SerializeField] private float velocitaBancoSuPlayer = 1f; // Regolabile da Inspector

    private GameObject playerOnTop;
    private Rigidbody2D playerRb;
    private Player playerScript;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void FixedUpdate()
    {
        if (playerOnTop != null && playerRb != null && playerScript != null && playerScript.isGrounded)
        {
            // Velocità del banco convertita in velocità fisica
            Vector2 bancoVel = (Vector2)(fishGroup.GetDeltaMovement() / Time.fixedDeltaTime);

            // Applica solo la velocità del banco al player (senza forze compensative)
            float aggiuntaX = bancoVel.x * velocitaBancoSuPlayer;
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x + aggiuntaX, playerRb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    playerOnTop = collision.gameObject;
                    playerRb = playerOnTop.GetComponent<Rigidbody2D>();
                    playerScript = playerOnTop.GetComponent<Player>();
                    fishGroup.ActivateMovement();
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == playerOnTop)
        {
            playerOnTop = null;
            playerRb = null;
            playerScript = null;
        }
    }
}
*/