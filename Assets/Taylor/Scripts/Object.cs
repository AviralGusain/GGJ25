using UnityEngine;

public class Object : MonoBehaviour
{
    private Camera cam;
    public bool hoverMode = false;
    private Inventory inventory;
    private int currentRotation = 0;        // Track the current rotation on the Y-axis
    private GridManager gridManager;        // Reference to GridManager
    private MeshRenderer blueprintRenderer; // Renderer for BlueprintLook's material'
    private ScreenShake screenShake;        // Reference to ScreenShake

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        cam = Camera.main;
        gridManager = FindObjectOfType<GridManager>();
        screenShake = Camera.main.GetComponent<ScreenShake>();

        // Find the BlueprintLook's MeshRenderer
        Transform blueprintLook = transform.Find("BlueprintLook");
        if (blueprintLook != null)
        {
            blueprintRenderer = blueprintLook.GetComponent<MeshRenderer>();
        }
        else
        {
            Debug.LogError("ErrorLog: BlueprintLook GameObject not found in ghost prefab!");
        }
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
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = !enable; // Disable collisions during hover
        }
    }

    private void FollowMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // Snap to the grid center
            Vector3 cellCenter = gridManager.GetCellCenter(hit.point);
            transform.position = cellCenter;

            // Check if the cell is occupied
            if (gridManager.IsCellOccupied(cellCenter))
            {
                // Turn red for invalid placement
                SetBlueprintColor(new Color(1f, 0f, 0f, 110 / 255f)); // Solid red

                // Try to place on an occupied cell
                if (Input.GetMouseButtonDown(0))
                {
                    // Trigger screen shake for invalid placement
                    if (screenShake != null)
                    {
                        screenShake.TriggerShake();
                    }
                }
            }
            else
            {
                // Turn blue for valid placement
                SetBlueprintColor(new Color(0f, 0f, 1f, 110 / 255f)); // Solid blue

                // Place the object
                if (Input.GetMouseButtonDown(0))
                {
                    hoverMode = false;
                    if (GetComponent<Collider>() != null)
                    {
                        GetComponent<Collider>().enabled = true;
                    }
                    inventory.PlaceItem();
                }
            }

            // Cancel placement (return to inventory)
            if (Input.GetMouseButtonDown(1))
            {
                inventory.CancelPlacement();
            }
        }
    }

    private void HandleRotationInput()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            // Scroll up: Rotate 90 degrees counterclockwise
            currentRotation = (currentRotation - 90 + 360) % 360; // Add 360 to avoid negative angles
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            // Scroll down: Rotate 90 degrees clockwise
            currentRotation = (currentRotation + 90) % 360;
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
    }

    private void HandleObjectDestruction()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePosition);

        bool canDestroy = false;
        if (FindFirstObjectByType<LevelStateManager>().IsInDebug() || // can destroy everything in debug
            gameObject.CompareTag("Bouncer") == true ||
            gameObject.CompareTag("Fan") == true ||
            gameObject.CompareTag("Launcher") == true)
        {
            canDestroy = true;
        }

        if (!canDestroy)
        {
            return; // No destruction possible, return early
        }

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && Input.GetMouseButtonDown(1))
        {
            if (hit.transform == transform)
            {
                Destroy(gameObject);
                inventory.DestroyItem(tag);
                FindFirstObjectByType<LevelStateManager>().mOnObjectPlaced.Invoke();
            }
        }
    }

    private void SetBlueprintColor(Color color)
    {
        if (blueprintRenderer != null)
        {
            blueprintRenderer.material.SetColor("_BaseColor", color); // Update material color
        }
        else
        {
            Debug.LogError("Blueprint Renderer not assigned or missing!");
        }
    }
}
