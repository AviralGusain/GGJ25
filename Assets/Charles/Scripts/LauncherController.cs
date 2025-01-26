using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LauncherController : MonoBehaviour
{
  private Vector3 finalPos; // The final position of the bubble

  public bool LaunchBubble(GameObject bubble, Vector3 dir, float moveSpeed, float dt)
  {
    Debug.Log("Launch Direction: " + dir);

    // Form parabolic trajectory
    Vector3 param = Vector3.Scale(dir, bubble.transform.position);

    float x = bubble.transform.position.x;
    float y = 0.0f;
    float z = bubble.transform.position.z;

    if (param.x == 0)
    {
      z = bubble.transform.position.z + dir.z * moveSpeed * Time.deltaTime;

      y = ParabolicArc(z);
    }
    else
    {
      x = bubble.transform.position.x + dir.x * moveSpeed * Time.deltaTime;
      y = ParabolicArc(x);
    }

    // Get rigidbody of bubble
    Rigidbody rb = bubble.GetComponent<Rigidbody>();

    Vector3 newPos = new Vector3(x, y, z);
    Debug.Log("New position: " + newPos);

    // Move bubble to new position
    rb.MovePosition(newPos);

    if ((transform.GetChild(0).position - bubble.transform.position).magnitude < 0.1f)
    {
      Debug.Log("Bubble has reached final position");
      bubble.transform.position = finalPos;
      return true;
    }

    return false;
  }

  public float ParabolicArc(float x)
  {
    Vector3 result = Vector3.zero;
   
    float y = - (5.0f / 8.0f) * Mathf.Pow(x - 2f, 2) + 2.5f;

    return y;
  }
}
