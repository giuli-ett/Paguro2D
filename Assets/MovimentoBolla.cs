using UnityEngine;

public class MovimentoBolla : MonoBehaviour
{
    public float ampiezza = 0.2f;
    public float frequenza = 1f;

    private Vector3 posizioneIniziale;

    void Start()
    {
        posizioneIniziale = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequenza) * ampiezza;
        transform.position = posizioneIniziale + new Vector3(0f, offsetY, 0f);
    }
}
