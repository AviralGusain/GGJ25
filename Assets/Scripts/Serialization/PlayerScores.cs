
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerScores : MonoBehaviour
{
    public List<PlayerLevelScoreData> mScores;

    static PlayerScores mInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mInstance == null) // If no instance made
        {
            mInstance = this;
        }
        else // Already an instance, destroy it
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        LevelSaver.LoadPlayerScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerLevelScoreData GetLevelScore(string levelName)
    {
        PlayerLevelScoreData score = mScores.Find((PlayerLevelScoreData currScoreData) => { if (currScoreData.levelName == levelName) return true; else return false; });

        if (score != null)
        {
            return score;
        }
        else
        {
            PlayerLevelScoreData emptyScore = new PlayerLevelScoreData();
            emptyScore.levelName = levelName;
            emptyScore.numBubbles = 0;
            mScores.Add(emptyScore);
            return emptyScore;
        }


        //print("PlayerScores:GetLevelScore: No score exists for level name " + levelName);

        //return null;
    }

    public void SetNewScore(string levelName, int newScore, bool onlyIfHigher)
    {
        PlayerLevelScoreData levelScoreData = GetLevelScore(levelName);
        if (levelScoreData != null)
        {
            if (onlyIfHigher)
            {
                if (levelScoreData.numBubbles >= newScore)
                {
                    return;
                }
            }

            levelScoreData.numBubbles = newScore;
        }
    }
}
