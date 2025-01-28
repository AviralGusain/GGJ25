using System;
using System.Collections;
using UnityEngine;

public class BubbleGoal : MonoBehaviour
{

  public Action<BubbleData> mOnBubbleReachedGoal;

    private float moveSpeed = 0.25f;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("BubbleGoal OnTriggerEnter");

    if (other.CompareTag("Bubble"))
    {
      // NOTE: Add check for being correct type of doing that. If doing that, if bubble is not of right type, pop it
      BubbleController bubbleController = other.GetComponent<BubbleController>();

      if (mOnBubbleReachedGoal != null)
      {
        mOnBubbleReachedGoal.Invoke(new BubbleData());
      }
            
            StartCoroutine(GoalSequence(bubbleController));

    }
  }

    private IEnumerator GoalSequence(BubbleController bubbleController)
    {
        Transform bubble = bubbleController.transform;

        bubbleController.direction = Vector3.zero;
        bubbleController.bubbleAnimator.SetTrigger("Exit");


        Vector3 targetPosition = new Vector3(transform.position.x, bubble.position.y, transform.position.z);
        float duration = (moveSpeed > 0.0f) ? (Vector3.Distance(bubble.position, targetPosition) / moveSpeed) : 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            bubble.position = Vector3.Lerp(bubble.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if(Vector3.Distance(bubble.position, targetPosition) <= 0.1f)
        {
            //ADD SOUND
            //bubble.GetComponent<AudioSource>().Play();

            // Replace "YourStateName" with the name of your animation state
            AnimatorStateInfo stateInfo = bubbleController.bubbleAnimator.GetCurrentAnimatorStateInfo(0);

            // Check if the animation is in a specific state and has finished
            if (stateInfo.IsName("Goal") && stateInfo.normalizedTime >= 1f)
            {
                Destroy(bubble.gameObject);
            }

        }
    }
}
