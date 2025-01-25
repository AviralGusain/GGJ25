using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public bool switchToLeft = true; // Whether the bouncer turns the bubble to the left

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bubble"))
        {
            Bubble bubble = other.GetComponent<Bubble>();
            if (bubble != null)
            {
                // Calculate the new direction relative to the bubble's current direction
                Vector3 currentDirection = bubble.transform.forward;
                float angle = switchToLeft ? -90f : 90f; // Left (-90) or Right (+90)
                Vector3 newDirection = Quaternion.Euler(0, angle, 0) * currentDirection;

                bubble.ChangeDirection(newDirection);
            }
        }
    }
}
