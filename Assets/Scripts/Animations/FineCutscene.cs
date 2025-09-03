using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FineCutscene : MonoBehaviour

{
    [Header("Tempo prima di avviare il primo livello")]
    public float tempoAttesa = 20f;

    void Start()
    {
        StartCoroutine(CaricaLivelloDopoAttesa());
    }

    private IEnumerator CaricaLivelloDopoAttesa()
    {
        yield return new WaitForSeconds(tempoAttesa);
        SceneManager.LoadSceneAsync(3);
        MusicPlayer.Instance.PlayLevel1Music();
        //Cursor.visible = false;
    }
}
