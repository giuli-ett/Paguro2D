using UnityEngine;

public class MedusaAnimazioni : MonoBehaviour
{
    private Animator animator;
    private bool isPlayerOnTop = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("playerOnTop", false); // Inizia in Idle
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
           animator.SetBool("playerOnTop", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            animator.SetBool("playerOnTop", false);
        }
    }
}
