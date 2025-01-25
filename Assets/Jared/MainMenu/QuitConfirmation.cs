using UnityEngine;
using UnityEngine.UI;

public class QuitConfirmation : MonoBehaviour
{
    [Header("Buttons")]
    public Button YesButton;
    public Button NoButton;

    private void Start()
    {
        YesButton.onClick.AddListener(Yes);
        NoButton.onClick.AddListener(No);
    }

    private void Update()
    {
        
    }

    public void Yes()
    {
        Application.Quit();
    }

    public void No()
    {
        this.gameObject.SetActive(false);
    }
}
