using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button CloseButton;
    private MainMenu MM;

    private void Start()
    {
        MM = FindAnyObjectByType<MainMenu>();
        CloseButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        MM.DestroySubmenus();
    }

    public void OpenLevel(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
