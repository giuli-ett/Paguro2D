using UnityEngine;
using UnityEngine.SceneManagement;

public class LivelliMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public void PlayLivelloUno()
    {
        GameManager.Instance.livello1Completato = false;
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(3);
        //Cursor.visible = false;
        MusicPlayer.Instance.PlayLevel1Music();
    }
    public void PlayLivelloDue()
    {
        GameManager.Instance.livello2Completato = false;
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(4);
        //Cursor.visible = false;
        MusicPlayer.Instance.PlayLevel2Music();   
    }
    public void PlayLivelloTre()
    {
        AudioManager.Instance.PlayClick();  
    }
    public void PlayLivelloQuattro()
    {
        AudioManager.Instance.PlayClick();  
    }
    
}
