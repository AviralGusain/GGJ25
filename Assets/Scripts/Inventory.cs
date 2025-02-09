using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject bouncerPrefab;
    public GameObject fanPrefab;
    public GameObject launcherPrefab;       // Launcher prefab
    public GameObject ghostBouncerPrefab;   // Ghost version of Bouncer
    public GameObject ghostFanPrefab;       // Ghost version of Fan
    public GameObject ghostLauncherPrefab;  // Ghost version of Launcher
    public GameObject baseGoalPrefab;

    public int bouncerInvCount;
    public int fanInvCount;
    public int launcherInvCount;            // Inventory count for Launcher

    public TextMeshProUGUI bouncerText;
    public TextMeshProUGUI fanText;
    public TextMeshProUGUI launcherText;    // UI for Launcher count

    private Button bouncerButton;
    private Button fanButton;
    private Button launcherButton;          // Launcher button

    private string selectedItem = null;     // Tracks the currently selected item
    private GameObject hoverObject = null;  // The object currently hovering with the mouse
    private bool hoverItemDeducted = false; // Tracks if the inventory count was deducted

    private void Start()
    {
        bouncerButton = bouncerText.GetComponentInParent<Button>();
        fanButton = fanText.GetComponentInParent<Button>();
        launcherButton = launcherText.GetComponentInParent<Button>();

        if (bouncerButton == null || fanButton == null || launcherButton == null)
            Debug.LogError("Inventory buttons are not assigned correctly!");

        UpdateCounterDisplays();
    }

    private void Update()
    {
        if (FindFirstObjectByType<LevelStateManager>().IsInDebug() && Input.GetKeyUp(KeyCode.G))
        {
            SelectItem("BaseGoal");
        }
        else if (FindFirstObjectByType<LevelStateManager>().IsInDebug() && Input.GetKeyUp(KeyCode.B))
        {
            SelectItem("Spawner");
        }
        else if (FindFirstObjectByType<LevelStateManager>().IsInDebug() && Input.GetKeyUp(KeyCode.W))
        {
            SelectItem("Wall");
        }
    }

    public void SelectItem(string itemName)
    {
        // If a different item is selected while another item is already selected
        if (selectedItem != null && selectedItem != itemName)
        {
            // Return the inventory count for the previously selected item
            if (hoverItemDeducted)
            {
                if (selectedItem == "Bouncer") bouncerInvCount++;
                else if (selectedItem == "Fan") fanInvCount++;
                else if (selectedItem == "Launcher") launcherInvCount++;
            }

            // Destroy the current hover object
            if (hoverObject != null)
            {
                Destroy(hoverObject);
            }

            // Reset the hover deduction flag
            hoverItemDeducted = false;
        }

        // Proceed to select the new item
        if ((itemName == "Bouncer" && bouncerInvCount > 0) ||
            (itemName == "Fan" && fanInvCount > 0) ||
            (itemName == "Launcher" && launcherInvCount > 0) ||  // Launcher selection
            FindFirstObjectByType<LevelStateManager>().IsInDebug() && itemName == "BaseGoal" ||
            FindFirstObjectByType<LevelStateManager>().IsInDebug() && itemName == "Wall" ||
            FindFirstObjectByType<LevelStateManager>().IsInDebug() && itemName == "Spawner"
        )
        {
            selectedItem = itemName;

            // Deduct inventory count for hover
            if (hoverObject == null)
            {
                if (itemName == "Bouncer" && bouncerInvCount > 0)
                {
                    bouncerInvCount--;
                    hoverItemDeducted = true; // Mark as deducted
                }
                else if (itemName == "Fan" && fanInvCount > 0)
                {
                    fanInvCount--;
                    hoverItemDeducted = true; // Mark as deducted
                }
                else if (itemName == "Launcher" && launcherInvCount > 0)
                {
                    launcherInvCount--;
                    hoverItemDeducted = true; // Mark as deducted
                }
            }

            UpdateCounterDisplays();

            // Spawn a ghost hover object
            SpawnHoverObject(itemName);
        }
    }


    private void SpawnHoverObject(string itemName)
    {
        if (hoverObject != null)
            Destroy(hoverObject);

        GameObject prefab = null;

        // Use ghost versions for hovering
        switch (itemName)
        {
            case "Bouncer":
                prefab = ghostBouncerPrefab;
                break;
            case "Fan":
                prefab = ghostFanPrefab;
                break;
            case "Launcher":
                prefab = ghostLauncherPrefab;
                break;
            case "BaseGoal":
                prefab = baseGoalPrefab;
                break;
            case "Spawner":
                prefab = FindFirstObjectByType<GridManager>().spawnerPrefab;
                break;
            case "Wall":
                prefab = FindFirstObjectByType<GridManager>().GetRandomWallPrefab();
                break;
        }

        if (prefab == null)
        {
            Debug.LogError($"Prefab for {itemName} is not assigned in the Inventory!");
            return;
        }

        hoverObject = Instantiate(prefab, new Vector3(0, -1000, 0), Quaternion.identity); // Spawn offscreen

        // Enable hover mode
        Object objectScript = hoverObject.GetComponent<Object>();
        if (objectScript != null)
        {
            objectScript.SetHoverMode(true);
        }
        else
        {
            Debug.LogError($"The prefab for {itemName} does not have the Object script attached!");
        }
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
            case "Bouncer":
                realPrefab = bouncerPrefab;
                break;
            case "Fan":
                realPrefab = fanPrefab;
                break;
            case "Launcher":
                realPrefab = launcherPrefab;
                break;
            case "Wall":
                realPrefab = FindFirstObjectByType<GridManager>().GetRandomWallPrefab();
                break;
            case "Spawner":
                realPrefab = FindFirstObjectByType<GridManager>().spawnerPrefab;
                break;
            case "BaseGoal":
                realPrefab = FindFirstObjectByType<GridManager>().baseGoalPrefab;
                break;
        }

        if (realPrefab != null)
        {
            GameObject placedObject = FindFirstObjectByType<GridManager>().PlaceObjectAtPosition(position, realPrefab);
            if (placedObject != null) // if object is bnull, likely tried to place over an existing object
            {
                placedObject.transform.rotation = rotation;
            }

            FindFirstObjectByType<LevelStateManager>().mOnObjectPlaced.Invoke();
            FindFirstObjectByType<LevelStateManager>().ObjectPlaced(placedObject);
        }

        // Reset hover item state
        hoverObject = null;
        selectedItem = null;
        hoverItemDeducted = false; // Reset deduction flag
    }

    public void CancelPlacement()
    {
        if (hoverObject != null)
        {
            Destroy(hoverObject);

            // Increment inventory count back if deducted
            if (hoverItemDeducted)
            {
                if (selectedItem == "Bouncer") bouncerInvCount++;
                if (selectedItem == "Fan") fanInvCount++;
                if (selectedItem == "Launcher") launcherInvCount++;

                hoverItemDeducted = false; // Reset flag
                UpdateCounterDisplays();
            }
        }

        hoverObject = null;
        selectedItem = null;
    }

    public void DestroyItem(string itemName)
    {
        // Increment inventory count when an item is picked up from the grid
        if (itemName == "Bouncer")
        {
            bouncerInvCount++;
        }
        else if (itemName == "Fan")
        {
            fanInvCount++;
        }
        else if (itemName == "Launcher")
        {
            launcherInvCount++;
        }

        FindFirstObjectByType<LevelStateManager>().mOnObjectPlaced.Invoke();

        UpdateCounterDisplays();
    }

    private void UpdateCounterDisplays()
    {
        bouncerText.text = "x" + bouncerInvCount;
        fanText.text = "x" + fanInvCount;
        launcherText.text = "x" + launcherInvCount;

        bouncerButton.interactable = bouncerInvCount > 0;
        fanButton.interactable = fanInvCount > 0;
        launcherButton.interactable = launcherInvCount > 0;
    }

    public void SetNumBouncers(int numBouncers)
    {
        bouncerInvCount = numBouncers;
        UpdateCounterDisplays();
    }

    public void SetNumFans(int numFans)
    {
        fanInvCount = numFans;
        UpdateCounterDisplays();
    }

    public void SetNumLaunchers(int numLaunchers)
    {
        launcherInvCount = numLaunchers;
        UpdateCounterDisplays();
    }
}