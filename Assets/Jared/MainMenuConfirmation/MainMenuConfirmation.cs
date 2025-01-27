using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuConfirmation : MonoBehaviour
{
    [Header("Buttons")]
    public Button YesButton;
    public Button NoButton;

    private PauseMenu PM;

    private void Start()
    {
        PM = FindAnyObjectByType<PauseMenu>();

        YesButton.onClick.AddListener(Yes);
        NoButton.onClick.AddListener(No);
    }

    private void Update()
    {

    }

    public void Yes()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

        if (FindAnyObjectByType<WinScreen>() != null)
        {
            Destroy(this.gameObject);
        }
    }

    public void No()
    {
        if (PM != null)
            PM.DestroySubmenus();

        if (FindAnyObjectByType<WinScreen>() != null)
        {
            Destroy(this.gameObject);
        }  
    }
}
