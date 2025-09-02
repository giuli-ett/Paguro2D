using UnityEngine;

public class SuonoFantasma : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip fantasma;
    
    public void StartSound()
    {
        if (audioSource.isPlaying && audioSource.clip == fantasma)
            return;

        audioSource.clip = fantasma;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void StopSound()
    {
        if (audioSource.clip == fantasma)
            audioSource.Stop();
    }
}
