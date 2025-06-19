using UnityEngine;

public class CheckPointAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && animator != null)
        {
            AudioManager.Instance.PlayCheckPoint();
            animator.SetBool("playerInTrigger", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && animator != null)
        {
            animator.SetBool("playerInTrigger", false);
        }
    }
}