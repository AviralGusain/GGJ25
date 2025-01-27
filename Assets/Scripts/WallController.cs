using UnityEngine;

public class WallController : MonoBehaviour
{

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Bubble"))
    {
      // Play audio source attached to wall
      gameObject.GetComponent<AudioSource>().Play();

      Destroy(other.gameObject);
    }
  }
}
