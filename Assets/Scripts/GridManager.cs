using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 1f;
    public GameObject bouncerPrefab;
    public GameObject fanPrefab;
    public GameObject highlight;

    private GameObject[,] grid;

    void Start()
    {
        grid = new GameObject[gridWidth, gridHeight];
        DrawGrid();
    }

    void Update()
    {
        HighlightCell();
    }

    //void DrawGrid()
    //{
    //    // Draw grid lines for visualization
    //    for (int x = 0; x <= gridWidth; x++)
    //    {
    //        for (int y = 0; y <= gridHeight; y++)
    //        {
    //            Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
    //            Debug.DrawLine(position, position + Vector3.right * cellSize, Color.gray, 100f);
    //            Debug.DrawLine(position, position + Vector3.up * cellSize, Color.gray, 100f);
    //        }
    //    }
    //}

    void DrawGrid()
    {
        for (int x = 0; x <= gridWidth; x++)
        {
            for (int z = 0; z <= gridHeight; z++)
            {
                Vector3 start = new Vector3(x * cellSize, 0, z * cellSize); // X-Z plane
                Vector3 endX = start + new Vector3(0, 0, cellSize);         // Forward line
                Vector3 endZ = start + new Vector3(cellSize, 0, 0);         // Right line

                Debug.DrawLine(start, endX, Color.gray, 100f);
                Debug.DrawLine(start, endZ, Color.gray, 100f);
            }
        }
    }


    //Draw in game
    //void DrawGrid()
    //{
    //    for (int x = 0; x < gridWidth; x++)
    //    {
    //        for (int y = 0; y < gridHeight; y++)
    //        {
    //            Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
    //            GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad); // Use a Quad or Plane
    //            cell.transform.position = position + new Vector3(cellSize / 2, cellSize / 2, 0); // Center cells
    //            cell.transform.localScale = Vector3.one * cellSize; // Scale to match cell size
    //            cell.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.5f); // Light transparent color
    //        }
    //    }
    //}


    public Vector3 GetCellCenter(Vector3 worldPosition)
    {
        // Convert world position to grid indices (X-Z plane)
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int z = Mathf.FloorToInt(worldPosition.z / cellSize);

        // Clamp indices to stay within the grid
        x = Mathf.Clamp(x, 0, gridWidth - 1);
        z = Mathf.Clamp(z, 0, gridHeight - 1);

        // Return the center of the cell on the X-Z plane
        return new Vector3(x * cellSize + cellSize / 2, 0, z * cellSize + cellSize / 2);
    }


    public void PlaceObject(Vector3 worldPosition, GameObject prefab)
    {
        // Raycast to determine the grid cell
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Snap the hit point to the nearest cell center
            Vector3 cellCenter = GetCellCenter(hit.point);

            // Calculate grid indices
            int x = Mathf.FloorToInt(cellCenter.x / cellSize);
            int z = Mathf.FloorToInt(cellCenter.z / cellSize);

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
        // Get the mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Snap the hit point to the nearest cell center
            Vector3 cellCenter = GetCellCenter(hit.point);

            // Move the highlight object to the cell center
            if (highlight != null)
            {
                highlight.transform.position = cellCenter;
                highlight.SetActive(true); // Make sure it’s visible
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
