using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioMixer AudioMixer;

    static AudioManager audioManagerInstance;

    private void Start()
    {
        if (audioManagerInstance == null) // If no instance made
        {
            audioManagerInstance = this;
        }
        else // Already an instance, destroy it
        {
            Destroy(gameObject);
            return;
        }

        AudioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        AudioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        AudioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

    }
}
