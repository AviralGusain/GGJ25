using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering.UI;

public enum BubbleSize
{
    Normal
}

public enum BubbleColor
{
    Blue,
    NumColors
}


public struct BubbleData
{
    public BubbleSize mSize;
    public BubbleColor mColor;
}




public class LevelStateManager : MonoBehaviour
{
    public enum LevelState
    {
        Active,
        Complete
    }

    // Temp level variables to be moved into a modular level class
    // Public variables
    public List<int> mBubbleScoreTargets = new List<int>(); // target score of each bubble type

    public List<int> mCurrBubbleScores = new List<int>();

    public Action mOnObjectPlaced;
    // private variables

    public int mNumStartingBouncers;
    public int mNumStartingFans;
    public int mNumStartingLaunchers;

    LevelState mCurrState = LevelState.Active;

    bool mInDebug = true;

    bool mHasLoadedTestLevel = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitLevel();
        mOnObjectPlaced += ObjectPlaced;
    }

    // Update is called once per frame
    void Update()
    {
         // RILEY NOTE: Load test level, just for now
        if (mHasLoadedTestLevel == false)
        {
            LevelSaver.LoadLevel("TestLevel2", FindFirstObjectByType<GridManager>(), this); // RILEY NOTE: Start with test level, for testing. Change this when loading actual levels
            mOnObjectPlaced.Invoke();
            mHasLoadedTestLevel = true;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            LevelSaver.SaveCurrentLevel(FindFirstObjectByType<GridManager>(), this);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            GridManager grid = FindFirstObjectByType<GridManager>();

            LevelSaver.LoadLevel("TestLevel2", grid, FindFirstObjectByType<LevelStateManager>());
            mOnObjectPlaced.Invoke();

        }
    }

    // Public interface
    public void AddScoreFromBubble(BubbleData bubbleData)
    {
        mCurrBubbleScores[(int)bubbleData.mColor] += 1;

        print("Score increased, current score " + mCurrBubbleScores[(int)bubbleData.mColor].ToString());

        if (mCurrState != LevelState.Complete && IsLevelComplete())
        {
            print("You Win!");

            mCurrState = LevelState.Complete;
        }
    }

    // helper functions
    void InitLevel()
    {
        for (int i = mBubbleScoreTargets.Count; i < (int)BubbleColor.NumColors; i++)
        {
            mBubbleScoreTargets.Add(0); // Set score for this color to 0
        }

        mCurrBubbleScores.Clear();
        for (int i = 0; i < (int)BubbleColor.NumColors; i++)
        {
            mCurrBubbleScores.Add(0); // Set score for this color to 0
        }


        // Subscribe on pop events for the bubble goals so we score the bubbles they interact with
        var goals = FindObjectsByType<BubbleGoal>(FindObjectsSortMode.None);

        foreach (var goal in goals)
        {
            goal.mOnBubbleReachedGoal += AddScoreFromBubble;
        }


    }


    bool IsLevelComplete()
    {
        for (int i = 0; i < (int)BubbleColor.NumColors; i++)
        {
            if (mCurrBubbleScores[i] < mBubbleScoreTargets[i]) // if current count of current color is less than target
            {
                return false; // Not at target, level not complete, return false
            }
        }

        return true; // All requirements met, return true
    }

    public bool IsInDebug()
    {
        return mInDebug;
    }

    public void ObjectPlaced()
    {
        FanController[] fans = FindObjectsByType<FanController>(FindObjectsSortMode.None);
        foreach (var fan in fans)
        {
            fan.RecastFan();
        }
    }
}
