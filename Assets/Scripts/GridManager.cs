using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;        // Number of cells in the X direction
    public int gridHeight = 10;       // Number of cells in the Z direction
    public float cellSize = 1f;       // Size of each cell
    public GameObject bouncerPrefab;  // Bouncer prefab
    public GameObject fanPrefab;      // Fan prefab
    public GameObject highlight;      // Highlight object for mouse hover

    private GameObject[,] grid;       // 2D array to store grid objects
    public GameObject tempTilePrefab; // Temporary tile prefab for visualization
    public GameObject[] tilePrefabs;  // Array of tile prefabs

    private Vector3 gridOrigin; // The starting point of the grid

    void Start()
    {
        gridOrigin = transform.position; // Set the grid origin to the GridManager's position
        grid = new GameObject[gridWidth, gridHeight];
        DrawGrid();
    }

    void Update()
    {
        HighlightCell();
    }

    void DrawGrid()
    {
        // Draw the grid starting from the gridOrigin position
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Calculate the start and end positions for grid lines
                Vector3 start = gridOrigin + new Vector3(x * cellSize, 0, z * cellSize); // X-Z plane
                Vector3 endX = start + new Vector3(0, 0, cellSize); // Vertical grid line
                Vector3 endZ = start + new Vector3(cellSize, 0, 0); // Horizontal grid line

                // Calculate the position for the tile
                Vector3 tilePosition = gridOrigin + new Vector3(x * cellSize, 0, z * cellSize);

                // Choose a random tile prefab
                GameObject randomTilePrefab = GetRandomTilePrefab();

                // Instantiate the tile
                if (randomTilePrefab != null)
                {
                    Instantiate(
                        randomTilePrefab,
                        tilePosition + new Vector3(cellSize / 2, 0, cellSize / 2), // Center the tile
                        Quaternion.identity
                    );

                    // Draw grid lines for debugging
                    Debug.DrawLine(start, endX, Color.gray, 100f);
                    Debug.DrawLine(start, endZ, Color.gray, 100f);
                }
            }
        }
    }

    private GameObject GetRandomTilePrefab()
    {
        // Return a random prefab from the tilePrefabs array
        if (tilePrefabs != null && tilePrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, tilePrefabs.Length);
            return tilePrefabs[randomIndex];
        }

        Debug.LogError("Tile prefabs array is empty or not assigned!");
        return null;
    }

    public Vector3 GetCellCenter(Vector3 worldPosition)
    {
        // Convert the world position to local grid indices based on the gridOrigin
        Vector3 localPosition = worldPosition - gridOrigin; // Offset by the grid's origin
        int x = Mathf.FloorToInt(localPosition.x / cellSize);
        int z = Mathf.FloorToInt(localPosition.z / cellSize);

        // Clamp indices to ensure they're within grid bounds
        x = Mathf.Clamp(x, 0, gridWidth - 1);
        z = Mathf.Clamp(z, 0, gridHeight - 1);

        // Return the world position of the center of the cell
        return gridOrigin + new Vector3(x * cellSize + cellSize / 2, 0, z * cellSize + cellSize / 2);
    }

    public void PlaceObject(Vector3 worldPosition, GameObject prefab)
    {
        // Raycast to determine the grid cell
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Snap the hit point to the nearest cell center
            Vector3 cellCenter = GetCellCenter(hit.point);

            // Convert the world position to grid indices
            Vector3 localPosition = cellCenter - gridOrigin;
            int x = Mathf.FloorToInt(localPosition.x / cellSize);
            int z = Mathf.FloorToInt(localPosition.z / cellSize);

            // Check if the cell is empty
            if (grid[x, z] == null)
            {
                // Instantiate the object and register it in the grid
                GameObject placedObject = Instantiate(prefab, cellCenter, Quaternion.identity);
                grid[x, z] = placedObject;
            }
            else
            {
                Debug.Log("Cell is already occupied!");
            }
        }
    }

    void HighlightCell()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if the ray hits the grid plane
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Snap the hit point to the nearest cell center
            Vector3 cellCenter = GetCellCenter(hit.point);

            // Move the highlight object to the cell center
            if (highlight != null)
            {
                highlight.transform.position = cellCenter;
                highlight.SetActive(true); // Ensure the highlight is visible
            }
        }
        else
        {
            // Hide the highlight if the mouse is not over the grid
            if (highlight != null)
            {
                highlight.SetActive(false);
            }
        }
    }
}
