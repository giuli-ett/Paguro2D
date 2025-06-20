using UnityEngine;

public class TrasparenzaBabyCutscene : MonoBehaviour
{
    public string statoAnimazioneTarget = "StaticHide"; // Nome dello stato animazione che rende la sprite trasparente
    public float alphaTrasparente = 0f; // Valore di trasparenza (0 = invisibile, 1 = opaco)

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (animator == null || spriteRenderer == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(statoAnimazioneTarget))
        {
            SetAlpha(alphaTrasparente); // Rendi trasparente
        }
        else
        {
            SetAlpha(1f); // Rendi visibile
        }
    }

    void SetAlpha(float alpha)
    {
        Color colore = spriteRenderer.color;
        colore.a = alpha;
        spriteRenderer.color = colore;
    }
}