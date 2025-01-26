using NUnit.Framework;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public float RecordedTimeScale = 1f;

    private GameObject PauseMenuInstance = null;

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
        Time.timeScale = RecordedTimeScale;
        PauseMenuInstance.GetComponent<PauseMenu>().DestroySubmenus();
        Destroy(PauseMenuInstance);
    }
}
