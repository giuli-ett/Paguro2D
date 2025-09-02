using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource walkAudioSource;
    public AudioSource granchioAudioSource;
    public AudioSource oneShotAudioSource;

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
    public AudioClip jellyfishBounce;
    public AudioClip woodBreaking;
    public AudioClip melma;
    public AudioClip fantasma;
    public AudioClip granchio;

    [Header("GENERALI")]
    public AudioClip bubbles;
    public AudioClip click;
    public AudioClip navigateInventory;
    public AudioClip checkPoint;
    public AudioClip collezionabile;
    public AudioClip vittoria;
    public AudioClip crack;
    public AudioClip fishMoving;

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
        oneShotAudioSource.PlayOneShot(click);
    }
    public void PlayNavigateInventory()
    {
        oneShotAudioSource.PlayOneShot(navigateInventory);
    }

    public void PlaySalto()
    {
        oneShotAudioSource.PlayOneShot(salto);
    }
    public void StartWalking()
    {
        if (walkAudioSource.isPlaying && walkAudioSource.clip == walking)
            return;

        walkAudioSource.clip = walking;
        walkAudioSource.loop = true;
        walkAudioSource.Play();
    }
    public void StopWalking()
    {
        if (walkAudioSource.clip == walking)
            walkAudioSource.Stop();
    }

    public void PlayDig()
    {
        oneShotAudioSource.PlayOneShot(digging);
    }
    public void PlayDash()
    {
        oneShotAudioSource.PlayOneShot(dash);
    }
    public void PlayTrovaNuovoGuscio()
    {
        oneShotAudioSource.PlayOneShot(trovaNuovoGuscio);
    }
    public void PlayAperturaCassa()
    {
        oneShotAudioSource.PlayOneShot(aperturaCassa);
    }
    public void PlayDamage()
    {
        oneShotAudioSource.PlayOneShot(damage);
    }
    public void PlayDie()
    {
        oneShotAudioSource.PlayOneShot(die);
    }
    public void PlayJellyFishDamage()
    {
        oneShotAudioSource.PlayOneShot(jellyfishDamage);
    }
    public void PlayCollezionabile()
    {
        oneShotAudioSource.PlayOneShot(collezionabile);
    }
    public void PlayCheckPoint()
    {
        oneShotAudioSource.PlayOneShot(checkPoint);
    }
    public void PlayJellyfishBounce()
    {
        oneShotAudioSource.PlayOneShot(jellyfishBounce);
    }
    public void PlayBubbles()
    {
        oneShotAudioSource.PlayOneShot(bubbles);
    }
    public void PlayVittoria()
    {
        oneShotAudioSource.PlayOneShot(vittoria);
    }
    public void PlayCrack()
    {
        oneShotAudioSource.PlayOneShot(crack);
    }
    public void StopCrack()
    {
        if (oneShotAudioSource.clip == crack)
        {
            oneShotAudioSource.Stop();
        }
    }
    public void PlayWoodBreaking()
    {
        oneShotAudioSource.PlayOneShot(woodBreaking);
    }

    public void PlayMelma()
    {
        oneShotAudioSource.PlayOneShot(melma);
    }
    public void PlayFantasma()
    {
        oneShotAudioSource.PlayOneShot(fantasma);
    }
    public void PlayGranchio()
    {
        oneShotAudioSource.PlayOneShot(granchio);
    }

    public void PlayFishMoving()
    {
        oneShotAudioSource.PlayOneShot(fishMoving);
    } 
    
    public void StartGranchio()
    {
        if (granchioAudioSource.isPlaying && granchioAudioSource.clip == granchio)
            return;

        granchioAudioSource.clip = granchio;
        granchioAudioSource.loop = true;
        granchioAudioSource.Play();
    }
    public void StopGranchio()
    {
        if (granchioAudioSource.clip == granchio)
            granchioAudioSource.Stop();
    }
}
