using UnityEngine;
public class Pendolo : MonoBehaviour
{
    public float swingAngle = 30f; // Maximum swing angle
    public float swingSpeed = 1f;  // Speed of the swinging
    public float swingFrequency = 1f; // Frequency of the swing motion
    private float currentAngle;
    private float time;

    void Start()
    {
        // Ensure the rod is set at the initial position
        currentAngle = 0f;
        time = 0f;
    }
   void Update()
    {
        // Update the time based on swing speed
        time += Time.deltaTime * swingSpeed;
        // Calculate the current angle using a sine wave for smooth oscillation
        currentAngle = swingAngle * Mathf.Sin(time * swingFrequency);
        // Apply rotation
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
