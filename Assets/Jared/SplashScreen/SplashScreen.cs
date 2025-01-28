using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private float Timer = 0f;
    public float Duration = 4.25f;

    private void Start()
    {
        
    }

    private void Update()
    {
        Timer += Time.deltaTime;

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
