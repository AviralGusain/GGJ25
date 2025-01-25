using UnityEngine;

public class Fan : MonoBehaviour
{
    public float pushStrength = 1.5f; // Speed multiplier for bubbles

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bubble"))
        {
            Bubble bubble = other.GetComponent<Bubble>();
            if (bubble != null)
            {
                // Push the bubble in the fan's FORWARD direction
                Vector3 newDirection = transform.forward * pushStrength;
                bubble.ChangeDirection(newDirection);
            }
        }
    }
}
