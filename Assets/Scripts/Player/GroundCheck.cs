using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Assicurati che il player entri in contatto con un oggetto con tag "Hook"
        if (other.CompareTag("Player"))
        {
             Player.Instance.isGrounded = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.isGrounded = false;
        }
    }
}

