using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button CloseButton;


    private void Start()
    {
        CloseButton.onClick.AddListener(Close);
    }

    private void Update()
    {
        
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
