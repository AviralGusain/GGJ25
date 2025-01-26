using UnityEngine;

public class Object : MonoBehaviour
{
    private Camera cam;
    private bool hoverMode = false;
    private Inventory inventory;
    private int currentRotation = 0; // Track the current rotation on the Y-axis

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        cam = Camera.main;
    }

    void Update()
    {
        if (hoverMode)
        {
            FollowMousePosition();
            HandleRotationInput();
        }
        else
        {
            HandleObjectDestruction();
        }
    }

    public void SetHoverMode(bool enable)
    {
        hoverMode = enable;
        GetComponent<Collider>().enabled = !enable; // Disable collisions during hover
    }

    private void FollowMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // Snap to the grid center
            Vector3 cellCenter = FindObjectOfType<GridManager>().GetCellCenter(hit.point);
            transform.position = cellCenter;

            // Place the object
            if (Input.GetMouseButtonDown(0))
            {
                hoverMode = false;
                GetComponent<Collider>().enabled = true;
                inventory.PlaceItem();
            }

            // Cancel placement (return to inventory)
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(gameObject);
                inventory.DestroyItem(tag);
            }
        }
    }

    private void HandleRotationInput()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            // Scroll up: Rotate 90 degrees clockwise
            currentRotation = (currentRotation + 90) % 360;
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            // Scroll down: Rotate 90 degrees counterclockwise
            currentRotation = (currentRotation - 90 + 360) % 360; // Add 360 to avoid negative angles
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
    }

    private void HandleObjectDestruction()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && Input.GetMouseButtonDown(1))
        {
            if (hit.transform == transform)
            {
                Destroy(gameObject);
                inventory.DestroyItem(tag);
            }
        }
    }
}
