using UnityEngine;
public class FanController : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Bubble"))
    {
      // Play audio
      gameObject.GetComponent<AudioSource>().Play();

            Debug.Log("Fan");
            // Destroy the bubble
            other.GetComponent<BubbleController>().PopSequence();
    }
  }
}
