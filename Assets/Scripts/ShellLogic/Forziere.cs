using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Forziere : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform upPoint;
    public GameObject shellPrefab;
    public GameObject text;
    public bool isPlayerClose;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (text != null)
            {
                text.SetActive(true);
                Player.Instance.GetComponent<ForziereController>().closeForziere = this.gameObject;
                isPlayerClose = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (text != null)
            {
                text.SetActive(false);
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
            var shellPicker = newShell.GetComponent<ShellPicker>();
            Player.Instance.shellManager.WearShell(shellPicker.shell, shellPicker);
        });
    }
}
