using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed = 2f;                // Speed of movement
    public int travelDistance = 3;          // Number of grid cells to travel when spawned
    public bool limitTravelDistance = true; // Whether the bubble's movement should be limited

    private Vector3 direction = Vector3.right; // Initial movement direction
    private Vector3 targetPosition;            // The next grid cell the bubble is moving to
    private int cellsTraveled = 0;             // Counter for traveled cells

    void Start()
    {
        // Snap to the center of the starting grid cell
        targetPosition = GetCellCenter(transform.position);
        transform.position = targetPosition;

        // Calculate the first target position
        targetPosition += direction;
    }

    void Update()
    {
        // Move the bubble toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the bubble has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Snap to the target position
            transform.position = targetPosition;
            cellsTraveled++;

            // Check if movement should be limited and stop if necessary
            if (limitTravelDistance && cellsTraveled >= travelDistance)
            {
                Destroy(gameObject); // Destroy the bubble when it reaches the travel limit
                return;
            }

            // Update the target position to the next cell
            targetPosition += direction;
        }
    }

    public void ChangeDirection(Vector3 newDirection)
    {
        // Change the direction and reset target position to align with the grid
        direction = newDirection.normalized;
        targetPosition = transform.position + direction;
    }

    private Vector3 GetCellCenter(Vector3 position)
    {
        // Snap the position to the center of the nearest grid cell
        float cellSize = 1f; // Assuming grid cells are 1 unit in size
        float x = Mathf.Round(position.x / cellSize) * cellSize;
        float z = Mathf.Round(position.z / cellSize) * cellSize;
        return new Vector3(x, position.y, z);
    }
    public Vector3 GetDirection()
    {
        // Return the current direction of the bubble
        return direction;
    }
}
