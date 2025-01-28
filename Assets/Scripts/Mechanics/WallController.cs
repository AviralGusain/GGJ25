using UnityEngine;

public class WallController : MonoBehaviour
{

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Bubble"))
    {
            Debug.Log("wall");
      // Play audio source attached to wall
      gameObject.GetComponent<AudioSource>().Play();
      other.GetComponent<BubbleController>().PopSequence();
    }
  }
}
