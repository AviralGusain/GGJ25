
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerScores : MonoBehaviour
{
    public List<PlayerLevelScoreData> mScores;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

        print("PlayerScores:GetLevelScore: No score exists for level name " + levelName);

        return null;
    }
}
