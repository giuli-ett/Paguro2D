using UnityEngine;

public class SuonoBancoPesci : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip fishMoving;
    
    public void StartSound()
    {
        if (audioSource.isPlaying && audioSource.clip == fishMoving)
            return;

        audioSource.clip = fishMoving;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void StopSound()
    {
        if (audioSource.clip == fishMoving)
            audioSource.Stop();
    }
}
