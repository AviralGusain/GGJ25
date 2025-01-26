using System.Collections.Generic;
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
    public List<GameObject> Submenus;


    private void Start()
    {
        PlayButton.onClick.AddListener(Play);
        OptionsButton.onClick.AddListener(Options);
        CreditsButton.onClick.AddListener(Credits);
        QuitButton.onClick.AddListener(Quit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroySubmenus();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void Options()
    {
        Submenus.Add(Instantiate(OptionsMenu, this.gameObject.transform));
    }

    public void Credits()
    {
        Submenus.Add(Instantiate(CreditsMenu, this.gameObject.transform));
    }

    public void Quit()
    {
        Submenus.Add(Instantiate(QuitConfirmation, this.gameObject.transform));
    }

    public void DestroySubmenus()
    {
        for (int i = 0; i < Submenus.Count; i++)
        {
            Destroy(Submenus[i]);
        }

        Submenus.Clear();
    }
}
