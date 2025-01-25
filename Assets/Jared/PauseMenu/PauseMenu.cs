using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button ResumeButton;
    public Button OptionsButton;
    public Button MainMenuButton;
    public Button QuitButton;

    private PauseManager PM;

    private void Start()
    {
        PM = FindAnyObjectByType<PauseManager>();
    }

    private void Update()
    {
        
    }
}
