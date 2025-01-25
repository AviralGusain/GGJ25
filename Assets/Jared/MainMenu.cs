using UnityEngine;
using UnityEngine.UI;

public class MainMenu
{
    public Button PlayButton;
    public Button OptionsButton;
    public Button QuitButton;

    void Start()
    {
        PlayButton.onClick.AddListener(Play);

    }

    void Update()
    {

    }

    public void Play()
    {

    }

    public void Options()
    {
        
    }

    public void Quit()
    {
        //ADD CONFIRMATION

        Application.Quit();
    }
}
