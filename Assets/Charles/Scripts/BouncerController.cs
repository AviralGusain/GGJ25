using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BouncerController : MonoBehaviour
{
  public GameObject bouncer;
  private int orientation;

  public int GetReflectionPlane(Vector3 direction)
  {
    // Take rotation of the object along the y-axis
    float value = Mathf.Abs(bouncer.transform.rotation.eulerAngles.y) / 90.0f;

    Debug.Log("Orientation: " + value);

    orientation = (int)value;

    CollisionCheck(direction);

    return orientation;
  }

  private bool CollisionCheck(Vector3 direction)
  {
    // Make sure the direction passed in valid with the specified orientation
    Debug.Log("Direction: " + direction);

    // Orientation 0 (no movement down/right)

    // Orientation 1 (no movement down/left)

    // Orientation 2 (no movement up/left)

    // Orientation 3 (no movement up/right)

    return true;
  }
}
