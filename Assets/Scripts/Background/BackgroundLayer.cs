using UnityEngine;

[System.Serializable] public class BackgroundLayer : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private bool followPlayer;
    [SerializeField] private Transform camera;
    [SerializeField] private Transform player;
    [SerializeField] private float parallaxFactor;
    void Start()
    {
        if (followPlayer)
            startPos = player.position;
        else
            startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float distance = camera.position.x * parallaxFactor;
        if (followPlayer)
            transform.position = player.position;
        else
            transform.position = new Vector3(startPos.x + distance, transform.position.y, transform.position.z);
    }
}
