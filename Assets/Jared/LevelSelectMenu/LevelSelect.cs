using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button CloseButton;
    private MainMenu MM;

    private NextLevel nl;

    private void Start()
    {
        nl = FindAnyObjectByType<NextLevel>();
        MM = FindAnyObjectByType<MainMenu>();
        CloseButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        MM.DestroySubmenus();
    }

    public void OpenLevel(string SceneName)
    {
        if (SceneName == "Level1")
        {
            SceneManager.LoadScene("Tutorial");
            return;
        }

        nl.nextLevel = SceneName;
        SceneManager.LoadScene("LevelScene");
    }
}
