using TMPro;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public GameObject goalPrefab;
    public GameObject spawnerPrefab;
    public GameObject[] counterPrefab;
    public GameObject wandPrefab;


    private string selectedItem = null;    // Tracks the currently selected item
    private GameObject hoverObject = null; // The object currently hovering with the mouse


    private void Update()
    {
        if (FindFirstObjectByType<LevelStateManager>().IsInDebug() && Input.GetKeyUp(KeyCode.G))
        {
            SelectItem("BaseGoal");
        }
    }

    public void SelectItem(string itemName)
    {
        selectedItem = itemName;
    }

    public void PlaceItem()
    {
        if (hoverObject == null) return;

        // Replace the ghost version with the real version
        Vector3 position = hoverObject.transform.position;
        Quaternion rotation = hoverObject.transform.rotation;
        Destroy(hoverObject);

        GameObject realPrefab = null;

        switch (selectedItem)
        {
            case "BaseGoal":
                realPrefab = goalPrefab;
                break;
            case "Spawner":
                realPrefab = spawnerPrefab;
                break;
            case "Wall":
                realPrefab = counterPrefab[Random.Range(0, counterPrefab.Length)];
                break;
            case "Wand":
                realPrefab = wandPrefab;
                break;
        }

        if (realPrefab != null)
        {
            Instantiate(realPrefab, position, rotation); // Place the real object
        }

        hoverObject = null;
        selectedItem = null;
    }
}
