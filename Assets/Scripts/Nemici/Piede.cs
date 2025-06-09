using System.Collections;
using UnityEngine;

public class Piede : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float upperLimit = 5f;
    [SerializeField] private float lowerLimit = -5f;
    [SerializeField] private float pauseTime = 2f;

    private bool movingUp = true;
    private bool isPaused = false;

    void Update()
    {
        if (isPaused) return;

        Vector3 position = transform.position;

        if (movingUp)
        {
            position.y += speed * Time.deltaTime;
            if (position.y >= upperLimit)
            {
                position.y = upperLimit;
                StartCoroutine(PauseAtEdge());
                movingUp = false;
            }
        }
        else
        {
            position.y -= speed * Time.deltaTime;
            if (position.y <= lowerLimit)
            {
                position.y = lowerLimit;
                StartCoroutine(PauseAtEdge());
                movingUp = true;
            }
        }

        transform.position = position;
    }

    private IEnumerator PauseAtEdge()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        isPaused = false;
    }
}
