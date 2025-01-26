using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class FanController : MonoBehaviour
{
  public GameObject fan;

  public float castDistance = 10f;

  public bool first = true;

  private Transform airChild;
  private Transform colliderChild;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    airChild = fan.transform.GetChild(1);
    colliderChild = airChild.GetChild(0);
  }

  // Update is called once per frame
  void Update()
  {
    if (first)
    {
      Vector3 distance = Vector3.zero;

      // Perform a ray cast to check if any objects are hit
      RaycastHit hit;
      // Send the raycast from the edge of the fan geometry
      Vector3 startPosition = /*fan.transform.position + */airChild.position;
      Debug.Log("Start position: " + startPosition);
      Debug.Log("Fan right: " + fan.transform.right);

      if (Physics.Raycast(startPosition, fan.transform.right, out hit, castDistance))
      {
        // Calculate the distance from the edge of the fan to the object hit
        Vector3 test = Vector3.Scale(hit.transform.localScale, fan.transform.right);
        Debug.Log("Test: " + test);

        distance = hit.point - startPosition;

        Debug.Log("Hit: " + hit.transform.name);
      }
      else
      {
        distance = (fan.transform.right * castDistance);
      }

      Debug.Log("Distance of raycast: " + distance);

      // Set the scale of the air child to the distance of the raycast
      Vector3 scale = new Vector3(distance.magnitude, 1, 1);
      airChild.localScale = scale;

      first = false;
    }
  }

  private void OnTriggerEnter(Collider other)
  {

    if (other.CompareTag("Bubble"))
    {
      Destroy(other.transform.gameObject);
    }
  }


}
