using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject bouncerPrefab;
    public GameObject fanPrefab;
    public GameObject ghostBouncerPrefab; // Ghost version of Bouncer
    public GameObject ghostFanPrefab;     // Ghost version of Fan
    public GameObject baseGoalPrefab;

    public int bouncerInvCount;
    public int fanInvCount;

    public TextMeshProUGUI bouncerText;
    public TextMeshProUGUI fanText;

    private Button bouncerButton;
    private Button fanButton;

    private string selectedItem = null;    // Tracks the currently selected item
    private GameObject hoverObject = null; // The object currently hovering with the mouse

    private void Start()
    {
        bouncerButton = bouncerText.GetComponentInParent<Button>();
        fanButton = fanText.GetComponentInParent<Button>();

        if (bouncerButton == null || fanButton == null)
            Debug.LogError("Inventory buttons are not assigned correctly!");

        UpdateCounterDisplays();
    }

    private void Update()
    {
        if (FindFirstObjectByType<LevelStateManager>().IsInDebug() && Input.GetKeyUp(KeyCode.G))
        {
            SelectItem("BaseGoal");
        }
    }
    public void SelectItem(string itemName)
    {
        if ((itemName == "Bouncer" && bouncerInvCount > 0) ||
            (itemName == "Fan" && fanInvCount > 0) || 
            FindFirstObjectByType<LevelStateManager>().IsInDebug() && itemName == "BaseGoal") // If a debug level, and goal key pressed
        {
            selectedItem = itemName;

            // Spawn a hover object
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
            case "BaseGoal":
                prefab = baseGoalPrefab;
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
                bouncerInvCount--;
                break;
            case "Fan":
                realPrefab = fanPrefab;
                fanInvCount--;
                break;
        }

        if (realPrefab != null)
        {
            Instantiate(realPrefab, position, rotation); // Place the real object
        }

        UpdateCounterDisplays();
        hoverObject = null;
        selectedItem = null;
    }

    public void DestroyItem(string itemName)
    {
        if (itemName == "Bouncer")
        {
            bouncerInvCount++;
        }
        else if (itemName == "Fan")
        {
            fanInvCount++;
        }

        UpdateCounterDisplays();
    }

    private void UpdateCounterDisplays()
    {
        bouncerText.text = "x" + bouncerInvCount;
        fanText.text = "x" + fanInvCount;

        bouncerButton.interactable = bouncerInvCount > 0;
        fanButton.interactable = fanInvCount > 0;
    }
}
