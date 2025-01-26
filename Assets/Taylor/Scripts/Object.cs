using UnityEngine;

public class Object : MonoBehaviour
{
    private Camera cam;
    private bool hoverMode = false;
    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        cam = Camera.main;

        if (inventory == null)
            Debug.LogError("Inventory is not assigned or could not be found!");
        if (cam == null)
            Debug.LogError("Main Camera is not found!");
    }


    void Update()
    {
        if (hoverMode)
        {
            FollowMousePosition();
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
