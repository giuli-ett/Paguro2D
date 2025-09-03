using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForziereController : MonoBehaviour
{
    public GameObject closeForziere = null;
    public void Interact(InputAction.CallbackContext context)
    {
        if (closeForziere == null)
        {
            return;
        }

        bool isClose = closeForziere.GetComponent<Forziere>().isPlayerClose;

        if (context.started && isClose)
        {
            closeForziere.GetComponent<Animator>().SetBool("canOpen", true);
            AudioManager.Instance.PlayAperturaCassa();
            closeForziere.GetComponent<Forziere>().text.SetActive(false);

            if (context.started && isClose)
            {
                closeForziere.GetComponent<Animator>().SetBool("canOpen", true);
                AudioManager.Instance.PlayAperturaCassa();
                closeForziere.GetComponent<Forziere>().text.SetActive(false);

                if (TutorialManager.Instance != null && TutorialManager.Instance.tutorialMode)
                {
                    TutorialManager.Instance.tutorialMode = false;
                    TutorialManager.Instance.canvas.gameObject.SetActive(false);
                    TutorialManager.Instance.spriteMask.gameObject.SetActive(false);
                    TutorialManager.Instance.text.SetActive(false);
                    TutorialManager.Instance.GetComponent<BoxCollider2D>().enabled = false;

                    SpriteRenderer sr = TutorialManager.Instance.darkOverlay.GetComponent<SpriteRenderer>();
                    TutorialManager.Instance.StartCoroutine(TutorialManager.Instance.FadeOutSprite(sr));

                    Player.Instance.EnableMovement();
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Forziere"))
        {
            if (closeForziere != null && !closeForziere.GetComponent<Forziere>().sorpresa)
            {
                AudioManager.Instance.PlayTrovaNuovoGuscio();
                Player.Instance.animator.SetBool("newShell", true);
            }
        }
    }

    public void OnNewShellEnd()
    {
        Player.Instance.animator.SetBool("newShell", false);
    }
}
