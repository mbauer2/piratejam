using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 100f; // Adjust to your liking
    public Transform targetObject; // Assign your object in the Inspector
    public float minDelay = 1f; // Minimum delay in seconds before rotation starts
    public float maxDelay = 3f; // Maximum delay in seconds before rotation starts

    private float timer = 0f; // Timer to keep track of the delay
    private float startDelay; // Actual delay before rotation starts

    void Start()
    {
        // Assign a random delay within the specified range
        startDelay = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        // Ensure that targetObject is assigned
        if (targetObject != null)
        {
            // Increment the timer by the time elapsed since the last frame
            timer += Time.deltaTime;

            // Check if the timer has reached the start delay
            if (timer >= startDelay)
            {
                // Rotate the target object around its local Y axis (Vector3.up)
                targetObject.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            Debug.LogWarning("No target object assigned for rotation.");
        }
    }
}
