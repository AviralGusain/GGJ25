using System;
using UnityEngine;

public class BubbleGoal : MonoBehaviour
{

    public Action<BubbleData> mOnBubbleReachedGoal;

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
        if (other.CompareTag("Bubble"))
        {
            // NOTE: Add check for being correct type of doing that. If doing that, if bubble is not of right type, pop it

            other.GetComponent<BubbleController>().bubbleAnimator.SetTrigger("Exit");

            if (mOnBubbleReachedGoal != null)
            {
                mOnBubbleReachedGoal.Invoke(new BubbleData());
            }

            // Destroy bubble
            Destroy(other.gameObject);
        }
    }
}
