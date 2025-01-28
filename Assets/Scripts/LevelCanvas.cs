using UnityEngine;
using UnityEngine.UI;

public class LevelCanvas : MonoBehaviour
{
    public Button PauseButton;
    public Button ResetButton;
    private PauseManager PM;

    private void Start()
    {
        PauseButton.onClick.AddListener(Pause);
        ResetButton.onClick.AddListener(ResetButtonFunction);

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
}
