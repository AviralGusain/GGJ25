using UnityEngine;
using UnityEngine.UI;

public class QuitConfirmation : MonoBehaviour
{
    [Header("Buttons")]
    public Button YesButton;
    public Button NoButton;

    private MainMenu MM;
    private PauseMenu PM;

    private void Start()
    {
        MM = FindAnyObjectByType<MainMenu>();
        PM = FindAnyObjectByType<PauseMenu>();

        YesButton.onClick.AddListener(Yes);
        NoButton.onClick.AddListener(No);
    }

    private void Update()
    {
        
    }

    public void Yes()
    {
        Application.Quit();
        Debug.Log("Game closed :3");
    }

    public void No()
    {
        if (MM != null)
            MM.DestroySubmenus();

        if (PM != null)
            PM.DestroySubmenus();
    }
}
