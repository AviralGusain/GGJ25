using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum BubbleSize
{
    Normal
}

public enum BubbleColor
{
    Blue,
    NunColors
}


public struct BubbleData
{
    public BubbleSize mSize;
    public BubbleColor mColor;
}

public class LevelStateManager : MonoBehaviour
{
    // Temp level variables to be moved into a modular level class
    // Public variables
    public List<int> mBubbleScoreTargets = new List<int>(); // target score of each bubble type

    // private variables
    float mCurrScore = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Public interface
    public void AddScoreFromBubble(BubbleData bubbleData)
    {
        mBubbleScoreTargets[(int)bubbleData.mColor] += 1;
    }

    // helper functions
    void InitLevel()
    {

        for (int i = 0; i < (int)BubbleColor.NunColors; i++)
        {
            mBubbleScoreTargets.Add(0); // Set score for this color to 0
        }

        // Subscribe on pop events for the bubble goals so we score the bubbles they interact with
        var goals = FindObjectsByType<BubbleGoal>(FindObjectsSortMode.None);

        foreach (var goal in goals)
        {
            goal.mOnBubbleReachedGoal += AddScoreFromBubble;
        }


    }
}
