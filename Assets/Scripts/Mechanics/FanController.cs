using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FanController : MonoBehaviour
{
  public GameObject fan;

  private float castDistance = 10f;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    FindFirstObjectByType<LevelStateManager>().mOnObjectPlaced += RecastFan;
  }

  // Call Recast whenever object is placed
  public void Recast()
  {
    Debug.Log("Recasting fan");

    // Start by establishing the direction in which a ray will be cast
    Vector3 direction = transform.right;

    float distance = castDistance;

    // Cast a ray in the direction of the fan
    if (Physics.Raycast(transform.position, direction, out RaycastHit hit, castDistance))
    {
      // Print name of object that was hit
      Debug.Log("Hit object: " + hit.collider.gameObject.name);

      distance = hit.distance;

      Debug.Log("Hit distance: " + distance);
    }

    // Set the scale of the fan to the distance of the raycast
    fan.transform.localScale = new Vector3(distance, fan.transform.localScale.y, fan.transform.localScale.z);
    Debug.Log("Fan scale: " + fan.transform.localScale);
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
    Recast();
  }

}
