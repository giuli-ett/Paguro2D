using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;

    [Header("MOVIMENTO")]
    public AudioClip salto;
    public AudioClip walking;
    public AudioClip digging;
    public AudioClip dash;

    [Header("GUSCI")]
    public AudioClip trovaNuovoGuscio;
    public AudioClip aperturaCassa;

    [Header("DANNO")]
    public AudioClip damage;
    public AudioClip die;
    public AudioClip jellyfishDamage;

    [Header("GENERALI")]
    public AudioClip bubbles;
    public AudioClip click;
    public AudioClip navigateInventory;
    public AudioClip checkPoint;
    public AudioClip collezionabile;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClick()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(click);
    }
    public void PlayNavigateInventory()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(navigateInventory);
    }

    public void PlaySalto()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.PlayOneShot(salto);
    }
    public void StartWalking()
    {
        if (audioSource.isPlaying && audioSource.clip == walking)
            return;

        audioSource.clip = walking;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void StopWalking()
    {
        if (audioSource.clip == walking)
            audioSource.Stop();
    }

    public void PlayDig()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(digging);
    }
    public void PlayDash()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(dash);
    }
    public void PlayTrovaNuovoGuscio()
    {
        /*
        audioSource.loop = false;
        audioSource.PlayOneShot(trovaNuovoGuscio);
        */
        audioSource.clip = trovaNuovoGuscio;
        audioSource.loop = false;
        audioSource.Play();
    }
    public void PlayAperturaCassa()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(aperturaCassa);
    }
    public void PlayDamage()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(damage);
    }
    public void PlayDie()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(die);
    }
    public void PlayJellyFishDamage()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(jellyfishDamage);
    }
    public void PlayCollezionabile()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(collezionabile);
    }
    public void PlayCheckPoint()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(checkPoint);
    }
}
