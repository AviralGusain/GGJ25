using UnityEngine;

public class BubbleAnim_Test : MonoBehaviour
{
    public GameObject spawningBubble;
    public GameObject idleBubble;
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawningBubble.GetComponent<Animator>().SetTrigger("Spawn");
        }

        AnimatorStateInfo stateInfo = spawningBubble.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Spawning") && stateInfo.normalizedTime >= 1f)
        {
            finishedSpawning = true;
            idleBubble.SetActive(true);
            spawningBubble.SetActive(false);
        }
    }
}
