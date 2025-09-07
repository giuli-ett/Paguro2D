using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Forziere : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform upPoint;
    public GameObject shellPrefab;
    public GameObject textPC;
    public GameObject textController;
    public GameObject textToShow;
    public bool isPlayerClose;
    public bool sorpresa = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bool tutorialOk = TutorialManager.Instance == null || !TutorialManager.Instance.tutorialMode;

            if (GameManager.Instance.isUsingController)
            {
                textToShow = textController;
            }
            else
            {
                textToShow = textPC;
            }

            if (textToShow != null && tutorialOk)
            {
                textToShow.SetActive(true);
                Player.Instance.GetComponent<ForziereController>().closeForziere = this.gameObject;
                isPlayerClose = true;

                if (!sorpresa)
                {
                    sorpresa = true;
                    Player.Instance.animator.SetBool("newShell", true);
                }
            }
            else
            {
                Player.Instance.GetComponent<ForziereController>().closeForziere = this.gameObject;
                isPlayerClose = true;

                if (!sorpresa)
                {
                    sorpresa = true;
                    Player.Instance.animator.SetBool("newShell", true);
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (textToShow != null)
            {
                textToShow.SetActive(false);
                Player.Instance.GetComponent<ForziereController>().closeForziere = null;
                isPlayerClose = false;
            }
        }
    }

    public void OnForziereOpen()
    {
        GameObject newShell = Instantiate(shellPrefab, spawnPoint.position, Quaternion.identity);
        newShell.transform.localScale = Vector3.zero;

        Sequence moveShell = DOTween.Sequence();
        moveShell.Join(newShell.transform.DOScale(new Vector3(0.62f, 0.62f, 1f), 1f));
        moveShell.Join(newShell.transform.DOMove(upPoint.position, 1f));
        moveShell.AppendInterval(1f);

        moveShell.OnComplete(() =>
        {
            ShellPicker shellPicker = newShell.GetComponent<ShellPicker>();
            Debug.Log($"ShellPicker {shellPicker.name}");
            Player.Instance.shellManager.WearShell(shellPicker.shell, shellPicker);
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            Player.Instance.GetComponent<ForziereController>().closeForziere = null;
            
        });
    }
}
