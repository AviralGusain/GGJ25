using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button CloseButton;
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullscreenToggle;

    Resolution[] AllResolutions;

    private MainMenu MM;
    private PauseMenu PM;
    public AudioMixer AudioMixer;

    private void Start()
    {
        MM = FindAnyObjectByType<MainMenu>();
        PM = FindAnyObjectByType<PauseMenu>();

        CloseButton.onClick.AddListener(Close);

        //Resolution Dropdown
        AllResolutions = Screen.resolutions; // Where(resolution => resolution.refreshRateRatio.numerator / resolution.refreshRateRatio.denominator == 60).ToArray();

        ResolutionDropdown.ClearOptions();

        List<string> Options = new List<string>();

        int CurrentResolutionIndex = 0;
        for (int i = 0; i < AllResolutions.Length; i++)
        {
            string Option = AllResolutions[i].width + " x " + AllResolutions[i].height + " @" + AllResolutions[i].refreshRateRatio.numerator / AllResolutions[i].refreshRateRatio.denominator;
            Options.Add(Option);

            if (AllResolutions[i].width == Screen.width && AllResolutions[i].height == Screen.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(Options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

        //Fullscreen Toggle
        if (Screen.fullScreen)
        {
            FullscreenToggle.isOn = true;
        }
        else
        {
            FullscreenToggle.isOn = false;
        }
    }

    private void Update()
    {
        
    }

    public void Resolution(int ResolutionIndex)
    {
        Resolution resolution = AllResolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Fullscreen(bool IsFullscreen)
    {
        Screen.fullScreen = IsFullscreen;
    }

    public void MasterVolume(float volume)
    {
        AudioMixer.SetFloat("MasterVolume", volume);
    }

    public void MusicVolume(float volume)
    {
        AudioMixer.SetFloat("MusicVolume", volume);
    }

    public void SFXVolume(float volume)
    {
        AudioMixer.SetFloat("SFXVolume", volume);
    }

    public void Close()
    {
        if (MM != null)
            MM.DestroySubmenus();

        if (PM != null)
            PM.DestroySubmenus();
    }
}
