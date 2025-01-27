using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button CloseButton;

    private MainMenu MM;

    private void Start()
    {
        MM = FindAnyObjectByType<MainMenu>();

        CloseButton.onClick.AddListener(Close);
    }

    private void Update()
    {

    }

    public void Close()
    {
        MM.DestroySubmenus();
    }
}
