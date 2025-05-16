using UnityEngine;

public class MedusaAnimazioni : MonoBehaviour
{
    private Animator animator;
    private bool isPlayerOnTop = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("stato", 0); // Inizia in Idle
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (isPlayerOnTop)
        {
            animator.SetInteger("stato", 1); // Passa a Schiacciata
        }
        else if (stateInfo.IsName("Schiacciata"))
        {
            animator.SetInteger("stato", 2); // Passa a Rimbalzo
        }
        else if (stateInfo.IsName("Rimbalzo") && stateInfo.normalizedTime >= 1f)
        {
            animator.SetInteger("stato", 0); // Torna a Idle quando rimbalzo finisce
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerOnTop = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerOnTop = false;
        }
    }
}
