using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LauncherController : MonoBehaviour
{
  public float distanceTraveled = 0.0f;
  public Vector3 originalPosition = Vector3.zero;
  public Vector3 LaunchBubble(GameObject bubble, Vector3 dir, float moveSpeed, float dt, ref bool launching)
  {
    if (distanceTraveled == 0)
    {
      originalPosition = bubble.transform.position;
    }

    float x = 0.0f;
    float y = 0.0f;
    float z = 0.0f;

    if (dir.x == 0)
    {
      z = Mathf.Clamp(distanceTraveled, 0f, 4f); // Ensure x stays within range
      Debug.Log("Z: " + z);

      y = ParabolicArc(z);
      Debug.Log("Y: " + y);
    }
    else
    {
      x = Mathf.Clamp(distanceTraveled, 0f, 4f); // Ensure x stays within range
      Debug.Log("X: " + x);

      y = ParabolicArc(x);
      Debug.Log("Y: " + y);
    }
    distanceTraveled += moveSpeed * Time.deltaTime;

    Vector3 newPos = originalPosition + dir * distanceTraveled;
    newPos.y = y;


    if (distanceTraveled > 4.0f)
    {
      launching = false;
      distanceTraveled = 0;
      return transform.GetChild(0).position;
    }

    return newPos;
  }

  public float ParabolicArc(float x)
  {
    Vector3 result = Vector3.zero;
   
    float y = - (5.0f / 8.0f) * Mathf.Pow(x - 2f, 2) + 3.5f;

    return y;
  }
}
