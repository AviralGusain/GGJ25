using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;        // Number of cells in the X direction
    public int gridHeight = 10;       // Number of cells in the Z direction
    public float cellSize = 1f;       // Size of each cell
    public GameObject bouncerPrefab;  // Bouncer prefab
    public GameObject fanPrefab;      // Fan prefab
    public GameObject launcherPrefab;
    public GameObject baseGoalPrefab; // base goal (will look into how we want to do colored goals later)
    public GameObject wallPrefab;
    public GameObject spawnerPrefab;
    public GameObject highlight;      // Highlight object for mouse hover

    private GameObject[,] grid;       // 2D array to store grid objects
    public GameObject tempTilePrefab; // Temporary tile prefab for visualization
    public GameObject[] tilePrefabs;  // Array of tile prefabs

    private Vector3 gridOrigin; // The starting point of the grid

    public FileHandler.ArraySerializeWrapper<LevelItemPackage> levelSaveData;

    void Start()
    {
        gridOrigin = transform.position; // Set the grid origin to the GridManager's position
        grid = new GameObject[gridWidth, gridHeight];
        DrawGrid();
    }

    void Update()
    {
        //HighlightCell();
    }

    //void DrawGrid()
    //{
    //    // Draw the grid starting from the gridOrigin position
    //    for (int x = 0; x < gridWidth; x++)
    //    {
    //        for (int z = 0; z < gridHeight; z++)
    //        {
    //            // Calculate the start and end positions for grid lines
    //            Vector3 start = gridOrigin + new Vector3(x * cellSize, 0, z * cellSize); // X-Z plane
    //            Vector3 endX = start + new Vector3(0, 0, cellSize); // Vertical grid line
    //            Vector3 endZ = start + new Vector3(cellSize, 0, 0); // Horizontal grid line

    //            // Calculate the position for the tile
    //            Vector3 tilePosition = gridOrigin + new Vector3(x * cellSize, 0, z * cellSize);

    //            // Choose a random tile prefab
    //            GameObject randomTilePrefab = GetRandomTilePrefab();

    //            // Instantiate the tile
    //            if (randomTilePrefab != null)
    //            {
    //                Instantiate(
    //                    randomTilePrefab,
    //                    tilePosition + new Vector3(cellSize / 2, 0, cellSize / 2), // Center the tile
    //                    Quaternion.identity
    //                );

    //                // Draw grid lines for debugging
    //                Debug.DrawLine(start, endX, Color.gray, 100f);
    //                Debug.DrawLine(start, endZ, Color.gray, 100f);
    //            }
    //        }
    //    }
    //}

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

                // Instantiate the tile with a slight random Y-axis rotation
                if (randomTilePrefab != null)
                {
                    float randomRotationY = Random.Range(-5f, 5f); // Slight random rotation on Y-axis
                    Instantiate(
                        randomTilePrefab,
                        tilePosition + new Vector3(cellSize / 2, 0, cellSize / 2), // Center the tile
                        Quaternion.Euler(0, randomRotationY, 0) // Apply random Y-axis rotation
                    );

                    //// Draw grid lines for debugging
                    //Debug.DrawLine(start, endX, Color.gray, 100f);
                    //Debug.DrawLine(start, endZ, Color.gray, 100f);
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
                GameObject placeObject = Instantiate(prefab, cellCenter, Quaternion.identity);
                grid[x, z] = placeObject;
            }
            else
            {
                Debug.Log("Cell is already occupied!");
            }
        }
    }

    // Spawns a prefab at given location, snapped to the grid, and returns prefab spawned
    public GameObject PlaceObjectAtPosition(Vector3 worldPosition, GameObject prefab)
    {
        // Raycast to determine the grid cell
            // Snap the hit point to the nearest cell center
            Vector3 cellCenter = GetCellCenter(worldPosition);

            // Convert the world position to grid indices
            Vector3 localPosition = cellCenter - gridOrigin;
            int x = Mathf.FloorToInt(localPosition.x / cellSize);
            int z = Mathf.FloorToInt(localPosition.z / cellSize);

            // Check if the cell is empty
            if (grid[x, z] == null)
            {
                // Instantiate the object and register it in the grid
                GameObject placeObject = Instantiate(prefab, cellCenter, Quaternion.identity);
                grid[x, z] = placeObject;
                return placeObject;
            }
            else
            {
                Debug.Log("Cell is already occupied!");
                return null;
            }
    }


    public void PlaceAlreadySpawnedObject(Vector3 worldPosition, GameObject existingObject)
    {
        // Snap the hit point to the nearest cell center
        Vector3 cellCenter = GetCellCenter(worldPosition);

        // Convert the world position to grid indices
        Vector3 localPosition = cellCenter - gridOrigin;
        int x = Mathf.FloorToInt(localPosition.x / cellSize);
        int z = Mathf.FloorToInt(localPosition.z / cellSize);

        // Check if the cell is empty
        if (grid[x, z] == null)
        {

            // register it in the grid
            grid[x, z] = existingObject;
        }
        else
        {
            Debug.Log("Cell is already occupied!");
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


    // Test serialization functions
    public GameObject GetLevelGridItem(int width, int height)
    {
        return grid[width, height];
    }

    public void SetLevelSaveData(List<LevelItemPackage> saveData)
    {
        levelSaveData.mItems = saveData.ToArray();
    }

    public List<LevelItemPackage> GetLevelSaveData()
    {
        return new List<LevelItemPackage>(levelSaveData.mItems);
    }

    public GameObject GetPrefabByTagName(string name)
    {
        if (name == "Bouncer")
        {
            return bouncerPrefab;
        }
        else if (name == "Fan")
        {
            return fanPrefab;
        }
        else if (name == "Launcher")
        {
            return launcherPrefab; // Add when we have it
        }
        else if (name == "BaseGoal")
        {
            return baseGoalPrefab;
        }
        else if (name == "Spawner")
        {
            return spawnerPrefab;
        }
        else if (name == "Wall")
        {
            return wallPrefab;
        }


        print("GridManager:GetPrefabByTagName: Tag of name " + name + " did not correspond to a prefab in the grid manager");

        return null; // name did not match a prefab, return null
    }

    public void ResetGridToEmpty()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (grid[i, j] == null)
                {
                    continue;
                }

                Destroy(grid[i, j]); // Destroy existing object
                grid[i, j] = null; // Clear space on grid
            }
        }
    }

    // destroy, then re-create the grid, probably because we just loaded a level that could have a different size grid
    public void RebuildGrid()
    {
        // Destroy all tiles
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0;i < tiles.Length; i++)
        {
            Destroy(tiles[i]);
        }

        // Make a new grid with current values, usually
        gridOrigin = transform.position; // Set the grid origin to the GridManager's position
        grid = new GameObject[gridWidth, gridHeight]; // Make grid with current width and height
        DrawGrid();
    }
}
