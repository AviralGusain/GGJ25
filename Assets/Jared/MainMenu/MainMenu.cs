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
    public GameObject LevelSelectMenu;
    public GameObject OptionsMenu;
    public GameObject CreditsMenu;
    public GameObject QuitConfirmation;
    public List<GameObject> Submenus;

    public List<ParticleSystem> BubbleParticles;

    private void Start()
    {
        PlayButton.onClick.AddListener(Play);
        OptionsButton.onClick.AddListener(Options);
        CreditsButton.onClick.AddListener(Credits);
        QuitButton.onClick.AddListener(Quit);

        Submenus.Add(Instantiate(OptionsMenu, this.gameObject.transform));
        DestroySubmenus();
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
        Submenus.Add(Instantiate(LevelSelectMenu, this.gameObject.transform));
        PauseParticles();
    }

    public void Options()
    {
        Submenus.Add(Instantiate(OptionsMenu, this.gameObject.transform));
        PauseParticles();
    }

    public void Credits()
    {
        Submenus.Add(Instantiate(CreditsMenu, this.gameObject.transform));
        PauseParticles();
    }

    public void Quit()
    {
        Submenus.Add(Instantiate(QuitConfirmation, this.gameObject.transform));
    }

    public void PauseParticles()
    {
        for (int i = 0; i < BubbleParticles.Count; i++)
        {
            BubbleParticles[i].Stop();
        }
    }

    public void PlayParticles()
    {
        for (int i = 0; i < BubbleParticles.Count; i++)
        {
            BubbleParticles[i].Play();
        }
    }

    public void DestroySubmenus()
    {
        for (int i = 0; i < Submenus.Count; i++)
        {
            Destroy(Submenus[i]);
        }

        Submenus.Clear();
        PlayParticles();
    }
}
