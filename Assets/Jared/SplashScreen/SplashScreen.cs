using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private float Timer = 0f;
    public float Pop = 1f;
    public float Quack = 2.4f;
    public float Duration = 4.25f;

    private bool PopPlayed = false;
    private bool QuackPlayed = false;

    public AudioSource PopSource;
    public AudioSource QuackSource;

    private void Start()
    {
        
    }

    private void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= Pop)
        {
            if (!PopPlayed)
            {
                PopSource.Play();
                PopPlayed = true;
            }
        }

        if (Timer >= Quack)
        {
            if (!QuackPlayed)
            {
                QuackSource.Play();
                QuackPlayed = true;
            }
        }

        if (Timer >= Duration)
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
