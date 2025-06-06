using UnityEngine;

public class Forno : MonoBehaviour
{
    public Collider2D solidCollider;
    public PlatformEffector2D platformEffector;
    private Animator animator;
    public enum StatoForno { Chiuso, Aprendo, Aperto, Chiudendo }
    public StatoForno stato = StatoForno.Chiuso;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetStato(string nuovoStato)
    {
        switch (nuovoStato)
        {
            case "Aprendo":
                stato = StatoForno.Aprendo;
                solidCollider.enabled = true;
                platformEffector.enabled = false;
                break;

            case "Aperto":
                stato = StatoForno.Aperto;
                solidCollider.enabled = false;
                platformEffector.enabled = true;
                
                if (Player.Instance.isInForno)
                {
                    Player.Instance.canMove = true;
                    Player.Instance.spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                    Player.Instance.spriteRendererShell.color = new Color(1f, 1f, 1f, 1f);
                }
                break;

            case "Chiudendo":
                stato = StatoForno.Chiudendo;
                solidCollider.enabled = true;
                platformEffector.enabled = false;

                if (Player.Instance.isInForno)
                {
                    Player.Instance.canMove = false;
                    Player.Instance.spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                    Player.Instance.spriteRendererShell.color = new Color(1f, 1f, 1f, 0f);
                }
                break;

            case "Chiuso":
                stato = StatoForno.Chiuso;
                solidCollider.enabled = true;
                platformEffector.enabled = false;
                break;
        }
    }
}
