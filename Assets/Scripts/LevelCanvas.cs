using UnityEngine;
using UnityEngine.UI;

public class LevelCanvas : MonoBehaviour
{
    public Button PauseButton;
    public Button ResetButton;
    public Button CameraButton;
    private PauseManager PM;

    private void Start()
    {
        PauseButton.onClick.AddListener(Pause);
        ResetButton.onClick.AddListener(ResetButtonFunction);
        CameraButton.onClick.AddListener(Camera);

        PM = FindAnyObjectByType<PauseManager>();
    }

    private void Update()
    {
        
    }

    public void Pause()
    {
        PM.Pause();
    }

    public void ResetButtonFunction()
    {
        FindAnyObjectByType<LevelStateManager>().ResetCurrentLevel();
    }

    public void Camera()
    {
        FindAnyObjectByType<CameraManager>().Swap();
    }
}
