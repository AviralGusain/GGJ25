using UnityEngine;

public class InputController : MonoBehaviour
{
    public Inventory inventory;

    void Update()
    {
        // Left-click selects an inventory item
        if (Input.GetMouseButtonDown(0))
        {
            // Selection logic happens through the Inventory UI buttons
        }
    }
}
