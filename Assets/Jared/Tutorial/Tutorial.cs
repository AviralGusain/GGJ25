using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button NextButton;
    private int CurrentSlide = 1;

    public TextMeshProUGUI Caption;
    public Image Screenshot;

    public string Caption1;
    public string Caption2;
    public string Caption3;
    public Sprite Screenshot1;
    public Sprite Screenshot2;
    public Sprite Screenshot3;

    private NextLevel nl;

    private void Start()
    {
        nl = FindAnyObjectByType<NextLevel>();
        nl.nextLevel = "Level1";

        NextButton.onClick.AddListener(ChangeSlide);

        Caption.text = Caption1;
        Screenshot.sprite = Screenshot1;
    }

    private void Update()
    {
        
    }

    private void ChangeSlide()
    {
        if (CurrentSlide == 1)
        {
            Caption.text = Caption2;
            Screenshot.sprite = Screenshot2;
        }

        if (CurrentSlide == 2)
        {
            Caption.text = Caption3;
            Screenshot.sprite = Screenshot3;
        }

        if (CurrentSlide == 3)
        {
            nl.nextLevel = "Level1";
            SceneManager.LoadScene("LevelScene");
        }

        CurrentSlide++;
    }
}
