using UnityEngine;

public class SuonoSparaBolle : MonoBehaviour
{
    [Header("VARIABILI SUONO")]
    [SerializeField] private float distanzaAttivazione = 5;
    public bool isPlayerInRaggio = false;
    public AudioSource audioSource;
    public AudioClip suonoBolle;

    void Update()
    {
        ControllaVista();
    }

    public void ControllaVista()
    {
        float distanzaDalPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

        if (distanzaDalPlayer <= distanzaAttivazione)
        {
            if (!isPlayerInRaggio)
            {
                StartSound();
                isPlayerInRaggio = true;
            }
        }
        else
        {
            if (isPlayerInRaggio)
            {
                StopSound();
                isPlayerInRaggio = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Disegna il raggio di vista in scena per debug
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanzaAttivazione);
    }

    public void StartSound()
    {
        if (audioSource.isPlaying && audioSource.clip == suonoBolle)
            return;

        audioSource.clip = suonoBolle;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void StopSound()
    {
        if (audioSource.clip == suonoBolle)
            audioSource.Stop();
    }

}
