using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private float RecordedTimeScale = 1f;

    private GameObject PauseMenuInstance;

    public GameObject PauseMenuPrefab;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    public void Pause()
    {
        PauseMenuInstance = Instantiate(PauseMenuPrefab);
        RecordedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Destroy(PauseMenuInstance);
        Time.timeScale = RecordedTimeScale;
    }
}
