using UnityEngine;

public class AudioManager : MonoBehaviour
{
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

    }
    public void PlayDash()
    {

    }
    public void PlayTrovaNuovoGuscio()
    {

    }
    public void PlayAperturaCassa()
    {

    }
    public void PlayDamage()
    {

    }
    public void PlayDie()
    {

    }
    public void PlayJellyFishDamage()
    {

    }
}
