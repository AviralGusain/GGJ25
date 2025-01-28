using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BouncerController : MonoBehaviour
{
  public GameObject bouncer;
  private int orientation;

  private Animator animator;
    private ParticleSystem particle;

  private AudioSource[] audioSources;

  private void Start()
  {
        particle = GetComponentInChildren<ParticleSystem>();
    animator = GetComponentInChildren<Animator>();

    audioSources = GetComponents<AudioSource>();
  }

  public int GetReflectionPlane(Vector3 direction, ref bool possible)
  {
    int yAngle = (int)bouncer.transform.rotation.eulerAngles.y;

    // Make sure y angle is within 0-360
    while (yAngle < 0)
    {
      yAngle += 360;
    }
    if (yAngle >= 360)
    {
      yAngle %= 360;
    }

    // Take rotation of the object along the y-axis
    orientation = (yAngle / 90);

    possible = CollisionCheck(direction, orientation);

    if (!possible)
    {
      audioSources[0].Play();
    }

    return orientation;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Bubble"))
    {
      // Play audio source attached to bouncer
      audioSources[1].Play();
    }
  }

  private bool CollisionCheck(Vector3 direction, int orientation)
  {
    animator.SetTrigger("Bounce");
        particle.Play();

    // Make sure the direction passed in valid with the specified orientation
    switch (orientation)
    {
      // Orientation 0 (no movement down/right)
      case 0:
        if (direction == Vector3.right || direction == Vector3.back)
        {
          return false;
        }
        break;

      // Orientation 1 (no movement down/left)
      case 1:
        if (direction == Vector3.left || direction == Vector3.back)
        {
          return false;
        }
        break;

      // Orientation 2 (no movement up/left)
      case 2:
        if (direction == Vector3.left || direction == Vector3.forward)
        {
          return false;
        }
        break;

      // Orientation 3 (no movement up/right)
      case 3:
        if (direction == Vector3.right || direction == Vector3.forward)
        {
          return false;
        }
        break;

      default:
        break;
    }

    return true;
  }
}
