using UnityEngine;

public class InputController : MonoBehaviour
{
    public GridManager gridManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            gridManager.PlaceObject(mousePosition, gridManager.bouncerPrefab);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            gridManager.PlaceObject(mousePosition, gridManager.fanPrefab);
        }
    }
}
