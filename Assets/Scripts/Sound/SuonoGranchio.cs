using UnityEngine;

public class SuonoGranchio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip granchio;
    
    public void StartSound()
    {
        if (audioSource.isPlaying && audioSource.clip == granchio)
            return;

        audioSource.clip = granchio;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void StopSound()
    {
        if (audioSource.clip == granchio)
            audioSource.Stop();
    }
}
