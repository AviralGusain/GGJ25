using UnityEngine;

public class BubbleAnim_Test : MonoBehaviour
{
    public Animator animator;
    private bool finishedSpawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InitialSequence();
    }

    private void InitialSequence()
    {
        if (finishedSpawning) return;

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    animator.SetTrigger("Launch");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    animator.SetTrigger("Exit");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    animator.SetTrigger("Bounce");
        //}
    }
}
