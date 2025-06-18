using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    public AudioSource musicSource;
    public AudioClip menuMusic;
    public AudioClip level1;
    public AudioClip level2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste tra scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMenuMusic()
    {
        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayLevel1Music()
    {
        musicSource.clip = level1;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    public void PlayLevel2Music()
    {
        musicSource.clip = level2;
        musicSource.loop = true;
        musicSource.Play();
    }
}
