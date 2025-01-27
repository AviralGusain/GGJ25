using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FanController : MonoBehaviour
{
  public GameObject fan;

  public float castDistance = 10f;

  private bool first = true;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    FindFirstObjectByType<LevelStateManager>().mOnObjectPlaced += RecastFan;
  }

  // Update is called once per frame
  void Update()
  {
      Vector3 distance = Vector3.zero;

      Transform airChild = fan.transform.GetChild(1);

      // Perform a ray cast to check if any objects are hit
      RaycastHit hit;

      // Send the raycast from the edge of the fan geometry
      Vector3 startPosition = airChild.position;

      if (Physics.Raycast(startPosition, fan.transform.right, out hit, castDistance))
      {
        Debug.Log("Tag Hit on Raycast: " + hit.transform.root.tag);

        distance = hit.point - startPosition;
        Debug.Log("Hit Point: " + hit.point);
      }
      else
      {
        distance = (fan.transform.right * castDistance);
      }

      // Set the scale of the air child to the distance of the raycast
      Vector3 scale = new Vector3(distance.magnitude, 0, 0);
      airChild.localScale = scale;
  }

  private void OnDestroy()
  {
    LevelStateManager levelManager = FindFirstObjectByType<LevelStateManager>();
    if (levelManager != null)
    {
      levelManager.mOnObjectPlaced -= RecastFan;
    }
  }

  private void OnTriggerEnter(Collider other)
  {

    if (other.CompareTag("Bubble"))
    {
      gameObject.GetComponent<AudioSource>().Play();
      Destroy(other.transform.gameObject);
    }
  }

  public void RecastFan()
  {
    first = true;
  }

}