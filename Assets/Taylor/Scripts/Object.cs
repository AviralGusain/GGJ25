using Unity.VisualScripting;
using UnityEngine;

public class Object : MonoBehaviour
{
    private Camera cam;
    private bool followMouse = true;

    private Inventory inventory;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
        cam = Camera.main;
        followMouse = true;
    }

    void Update()
    {
        FollowMousePosition();
        HandleObjectDestruction();
    }

    private void FollowMousePosition()
    {
        if (!followMouse) return;

        Vector3 mousePosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            transform.position = hit.point;

            //place the object
            if (Input.GetMouseButtonDown(0))
            {
                followMouse = false;
                GetComponent<Collider>().enabled = true;
                //change this to snap to grid point
            }

            //delete the object
            if (Input.GetMouseButtonDown(1))
            {
                DestroyObject();
                //should get added back to inventory
            }
        }
    }

    private void HandleObjectDestruction()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(Input.GetMouseButtonDown(1) && hit.transform == transform)
            {
                DestroyObject();
                //should get added back to inventory
            }
        }
    }

    private void DestroyObject()
    {
        //tells the inventory to add it back into the inventory essentially
        inventory.DestroyObject(this.tag);

        Destroy(gameObject);
    }
}
