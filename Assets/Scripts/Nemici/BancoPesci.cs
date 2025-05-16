using UnityEngine;

public class BancoPesci : MonoBehaviour
{
    public Transform[] waypoints;
    [SerializeField] float speed = 2f;

    private int currentIndex = 0;

    private Vector3 lastPosition;
    private Vector3 deltaMovement;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Salva la posizione prima del movimento
        lastPosition = transform.position;

        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
                currentIndex = 0; // Loop
        }

        // Calcola il delta movimento
        deltaMovement = transform.position - lastPosition;
    }

    public Vector3 GetDeltaMovement()
    {
        return deltaMovement;
    }
}
