using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LauncherController : MonoBehaviour
{
  public Vector3 originalPosition = Vector3.zero;

  public float time = 0.0f;

  public Animator animator;

  private void Start()
  {
    animator = GetComponentInChildren<Animator>();
  }

  public Vector3 LaunchBubble(GameObject bubble, Vector3 dir, float moveSpeed, float dt, ref bool launching, ref float distanceTraveled)
  {
    animator.SetTrigger("Launch");

    time += dt;

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

      y = ParabolicArc(z);
    }
    else
    {
      x = Mathf.Clamp(distanceTraveled, 0f, 4f); // Ensure x stays within range

      y = ParabolicArc(x);
    }
    distanceTraveled += moveSpeed * dt;

    Vector3 newPos = originalPosition + dir * distanceTraveled;
    newPos.y = y;


    if (distanceTraveled > 4.0f)
    {
      launching = false;
      distanceTraveled = 0;
      bubble.GetComponent<Collider>().enabled = true;
      return transform.GetChild(0).position;
    }

    return newPos;
  }

  public float ParabolicArc(float x)
  {
    Vector3 result = Vector3.zero;

    float y = -(5.0f / 8.0f) * Mathf.Pow(x - 2f, 2) + 3.5f;

    return y;
  }
}
