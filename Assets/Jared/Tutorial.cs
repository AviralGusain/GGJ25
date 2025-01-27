using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private float TutorialTimer = 0f;
    public float TutorialTime = 3.5f;

    private void Start()
    {
        
    }

    private void Update()
    {
        TutorialTimer += Time.deltaTime;

        if (TutorialTimer >= TutorialTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindAnyObjectByType<AudioSource>().Play();
                Destroy(this.gameObject);
            }
        }
    }
}
