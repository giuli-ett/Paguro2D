using UnityEngine;

public class BackgroundLayer : MonoBehaviour
{
    private float lengthX;
    [SerializeField] private GameObject camera;
    [SerializeField] private Vector2 parallaxFactor;
    [SerializeField] private int tileCount = 5; // Set this to your number of tiles

    private float initialOffsetX;
    private float initialOffsetY;

    void Start()
    {
        initialOffsetX = transform.position.x;
        initialOffsetY = transform.position.y;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float camX = camera.transform.position.x;
        float camY = camera.transform.position.y;

        // Parallax movement
        float parallaxX = initialOffsetX + (camX - initialOffsetX) * parallaxFactor.x;
        float parallaxY = initialOffsetY + (camY - initialOffsetY) * parallaxFactor.y;

        // Calculate seamless horizontal looping using modulo
        float totalWidth = lengthX * tileCount;
        float relativeX = parallaxX - camX;
        float loopedX = camX + Mathf.Repeat(relativeX + totalWidth * 0.5f, totalWidth) - totalWidth * 0.5f;

        transform.position = new Vector3(loopedX, parallaxY, transform.position.z);
    }
}