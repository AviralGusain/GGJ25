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

    // private variables
    public List<int> mCurrBubbleScores = new List<int>();

    LevelState mCurrState = LevelState.Active;

    bool mInDebug = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            LevelSaver.SaveCurrentLevel(FindFirstObjectByType<GridManager>());
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            GridManager grid = FindFirstObjectByType<GridManager>();

            LevelSaver.LoadLevel("TestLevel", grid, FindFirstObjectByType<LevelStateManager>());
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
}
