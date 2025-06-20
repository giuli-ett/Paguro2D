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
        MusicPlayer.Instance.PlayLevel1Music();
        SceneManager.LoadSceneAsync(2); // Carica la scena con index 1
    }
}
