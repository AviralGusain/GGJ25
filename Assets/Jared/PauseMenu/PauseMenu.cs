using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button ResumeButton;
    public Button OptionsButton;
    public Button MainMenuButton;
    public Button QuitButton;

    [Header("Submenus")]
    public GameObject OptionsMenu;
    public GameObject MainMenuConfirmation;
    public GameObject QuitConfirmation;
    public List<GameObject> Submenus;

    private PauseManager PM;

    private void Start()
    {
        PM = FindAnyObjectByType<PauseManager>();

        ResumeButton.onClick.AddListener(Resume);
        OptionsButton.onClick.AddListener(Options);
        MainMenuButton.onClick.AddListener(MainMenu);
        QuitButton.onClick.AddListener(Quit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroySubmenus();
        }
    }

    public void Resume()
    {
        PM.Unpause();
    }

    public void Options()
    {
        Submenus.Add(Instantiate(OptionsMenu, this.gameObject.transform));
    }

    public void MainMenu()
    {
        Submenus.Add(Instantiate(MainMenuConfirmation, this.gameObject.transform));
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
