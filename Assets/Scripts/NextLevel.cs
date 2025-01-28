using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;
    public string lastLevel;
    public int numLevels = 10;

    public static NextLevel nextLevelInstance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (nextLevelInstance == null) // If no instance made
        {
            nextLevelInstance = this;
        }
        else // Already an instance, destroy it
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartToJustPlayedLevel()
    {
        NextLevel nextLevel = FindFirstObjectByType<NextLevel>();

        if (nextLevel != null)
        {
            nextLevel.nextLevel = lastLevel;

            SceneManager.LoadScene("LevelScene");

        }
    }

    public void StartNextLevel()
    {
        NextLevel nextLevel = FindFirstObjectByType<NextLevel>();

        if (nextLevel != null)
        {
            string lastLevelName = nextLevel.lastLevel;

            int levelNumber = int.Parse(lastLevelName.Substring(5));
            if (levelNumber < numLevels)
            {
                levelNumber++;
            }
            else
            {
                print("NextLevel:StartNextLevel: Tried to go to the next level but already at max level number " + levelNumber + " going to main menu instead");
                SceneManager.LoadScene("MainMenu");
                return;
            }

            nextLevel.nextLevel = "Level" + levelNumber;

            SceneManager.LoadScene("LevelScene");
        }
    }
}
