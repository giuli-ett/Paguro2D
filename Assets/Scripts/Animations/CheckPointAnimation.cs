using UnityEngine;

public class CheckPointAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D col2D;
    [SerializeField] private Color activatedColor = new Color(0.5f, 1f, 0.5f, 1f);

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
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

    public void OnAnimationEnd()
    {
        if (col2D != null)
        {
            col2D.enabled = false;
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = activatedColor;
        }
    }
}