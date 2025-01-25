using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BouncerController : MonoBehaviour
{
  public GameObject bouncer;
  private int orientation;

  public int GetReflectionPlane()
  {
    // Take rotation of the object along the y-axis
    float value = Mathf.Abs(bouncer.transform.rotation.eulerAngles.y) / 90.0f;

    Debug.Log("Orientation: " + value);

    orientation = (int)value; 

    return orientation;
  }
}
