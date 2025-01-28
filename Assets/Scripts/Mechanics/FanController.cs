using UnityEngine;
public class FanController : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Bubble"))
    {
      // Play audio
      gameObject.GetComponent<AudioSource>().Play();

      // Destroy the bubble
      Destroy(other.gameObject);
    }
  }
}
