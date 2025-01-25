using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button PlayButton;
    public Button OptionsButton;
    public Button CreditsButton;
    public Button QuitButton;

    [Header("Submenus")]
    public GameObject OptionsMenu;
    public GameObject CreditsMenu;
    public GameObject QuitConfirmation;

    private void Start()
    {
        PlayButton.onClick.AddListener(Play);
        OptionsButton.onClick.AddListener(Options);
        CreditsButton.onClick.AddListener(Credits);
        QuitButton.onClick.AddListener(Quit);
    }

    private void Update()
    {

    }

    public void Play()
    {
        SceneManager.LoadScene("AviScene");
    }

    public void Options()
    {
        OptionsMenu.SetActive(true);
    }

    public void Credits()
    {
        CreditsMenu.SetActive(true);
    }

    public void Quit()
    {
        QuitConfirmation.SetActive(true);
    }
}
