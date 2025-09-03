using UnityEngine;
using UnityEngine.SceneManagement;

public class LivelliMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public void PlayLivelloUno()
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(3);
        //Cursor.visible = false;
        MusicPlayer.Instance.PlayLevel1Music();
    }
    public void PlayLivelloDue()
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(4);
        //Cursor.visible = false;
        MusicPlayer.Instance.PlayLevel1Music();   
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
