using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public GameObject StarPrefab;
    public RectTransform StarPanel;
    public int StarCount;
    private List<GameObject> Stars = new List<GameObject>();

    public GameObject MainMenuConfirmation;
    public Button BackButton;
    public Button NextButton;

    public float BubblePause = 0.25f;
    public float BubbleTimer = 0f;
    public bool TimerStart = false;

    private void Start()
    {
        // Get player score
        PlayerScores scores = FindFirstObjectByType<PlayerScores>();
        NextLevel nextLevel = FindFirstObjectByType<NextLevel>();
        if (scores != null && nextLevel != null)
        {
            StarCount = scores.GetLevelScore(nextLevel.lastLevel).numBubbles;
        }

        if (StarCount >= 3)
            StarCount = 3;

        BackButton.onClick.AddListener(Back);
        NextButton.onClick.AddListener(NextLevel);

        for (int i = 0; i < StarCount; i++)
        {
            GameObject star = Instantiate(StarPrefab, StarPanel);
            Stars.Add(star);
        }
    }

    private void Update()
    {
        if (TimerStart)
        {
            ExplodeStars();

            BubbleTimer += Time.deltaTime;

            if (BubbleTimer >= BubblePause)
            {
                NextLevel nextLevelManager = FindFirstObjectByType<NextLevel>();
                if (nextLevelManager != null) // If next level manager exists (should always in game, but maybe not if you open up a specific scene in testing)
                {
                    nextLevelManager.StartNextLevel(); // Starts next level if there is one, or main menu if not
                    return;
                }
                //if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
                //{
                //    SceneManager.LoadScene("MainMenu");
                //}
                //else
                //{
                   
                //    else
                //    {
                //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                //    }
                //}
            }
        }
    }

    public void Back()
    {
        Instantiate(MainMenuConfirmation, this.gameObject.transform);
    }

    public void NextLevel()
    {
        TimerStart = true;
    }

    public void ExplodeStars()
    {
        for (int i = 0; i < Stars.Count; i++)
        {
            //Playanimation
            Stars[i].GetComponent<Animator>().SetTrigger("Explode");
        }
    }

}
