using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private float Timer = 0f;
    public float Time = 4;

    private void Start()
    {
        
    }

    private void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= Time)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
