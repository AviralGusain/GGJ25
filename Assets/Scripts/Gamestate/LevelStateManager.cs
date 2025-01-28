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

public enum ResourceType
{
    Bouncer,
    Fan,
    Launcher
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

    public List<int> mNumExcpectedResourceUses = new List<int>() { 0, 0, 0 };

    public List<int> mHowManyLessThanExpectedForBubbleScore = new List<int>() { 0, 0, 0 }; // Number of fewer resources used than the total expected count. first index is count for 1 bubble score, second for 2, 3rd for 3

    LevelState mCurrState = LevelState.Active;

    bool mInDebug = true;

    bool mHasLoadedTestLevel = false;

    string mCurrLevelName = "";

    GameObject playerScoresPrefab;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitLevel();
        mOnObjectPlaced += ObjectPlaced;

        //playerScoresPrefab = Resources.Load<GameObject>("Assets/Prefabs/PlayerScoresHolder");
    }

    // Update is called once per frame
    void Update()
    {
         // RILEY NOTE: Load test level, just for now
        if (mHasLoadedTestLevel == false)
        {
            //// if no player score manager, make one
            //if (FindFirstObjectByType<PlayerScores>() == null)
            //{
            //    Instantiate(playerScoresPrefab);
            //}

            NextLevel levelTransitionHelper = FindFirstObjectByType<NextLevel>();
            string levelNameToLoad = "TestLevel2";
            if (levelTransitionHelper != null)
            {
                levelNameToLoad = levelTransitionHelper.nextLevel;
            }
            LevelSaver.LoadLevel(levelNameToLoad, FindFirstObjectByType<GridManager>(), this); // RILEY NOTE: Start with test level, for testing. Change this when loading actual levels
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
            int levelScore = CalculateLevelScore();
            print("You Win! You got " + levelScore + " bubbles");

            PlayerScores scoresHolder = FindFirstObjectByType<PlayerScores>();
            if (scoresHolder != null)
            {
                scoresHolder.SetNewScore(mCurrLevelName, levelScore, true);
            }

            NextLevel nextLevel = FindFirstObjectByType<NextLevel>();
            if (nextLevel != null)
            {
                nextLevel.lastLevel = mCurrLevelName;
            }
            
            mCurrState = LevelState.Complete;

        }
    }

    // helper functions
    public void InitLevel()
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

        //mOnObjectPlaced.Invoke();
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

    int CalculateLevelScore()
    {
        int[] diffFromExpectedResources = { 0, 0, 0 };

        Inventory inventory = FindAnyObjectByType<Inventory>();

        // Get difference between what the player used, vs what was expected. The lower the difference, the better
        int numUsedBouncers = mNumStartingBouncers - inventory.bouncerInvCount;
        diffFromExpectedResources[(int)ResourceType.Bouncer] = numUsedBouncers - mNumExcpectedResourceUses[(int)ResourceType.Bouncer];

        int numUsedFans = mNumStartingFans - inventory.fanInvCount;
        diffFromExpectedResources[(int)ResourceType.Fan] = numUsedFans - mNumExcpectedResourceUses[(int)ResourceType.Fan];

        int numUsedLaunchers = mNumStartingLaunchers - inventory.launcherInvCount;
        diffFromExpectedResources[(int)ResourceType.Launcher] = numUsedLaunchers - mNumExcpectedResourceUses[(int)ResourceType.Launcher];

        //int totalExpectedResourceUses = 0;
        //int totalUsedResources = 0;

        int totalLessThanExpectedResources = 0; // Difference between expected resource uses, and actual uses

        foreach (int count in  diffFromExpectedResources)
        {
            totalLessThanExpectedResources -= count;
        }

        for (int i = mHowManyLessThanExpectedForBubbleScore.Count - 1; i >= 0; i--)
        {
            int currScoreDiffThreshold = mHowManyLessThanExpectedForBubbleScore[i];
            if (totalLessThanExpectedResources >= currScoreDiffThreshold)
            {
                return i + 1; // number of bubbles (+1 because index is 0 based)
            }
        }

        return 0; // If reaching here, did not meet any requirements, and level score was set properly

    }

    public bool IsInDebug()
    {
        return mInDebug;
    }

    public void ObjectPlaced()
    {
        WindController[] fans = FindObjectsByType<WindController>(FindObjectsSortMode.None);
        foreach (var fan in fans)
        {
            fan.RecastFan();
        }
    }

    public void SetCurrLevelName(string name)
    {
        mCurrLevelName = name;
    }

    public void ReInitLevel()
    {
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

        mCurrState = LevelState.Active;
    }

    public void ObjectPlaced(GameObject newObject)
    {
        if (newObject.CompareTag("BaseGoal"))
        {
            newObject.GetComponent<BubbleGoal>().mOnBubbleReachedGoal += AddScoreFromBubble;
        }
    }

    public void ResetCurrentLevel()
    {
        LoadLevel(mCurrLevelName);
    }

    public void SaveLevel(string levelName)
    {
        LevelSaver.SaveCurrentLevel(FindFirstObjectByType<GridManager>(), this, levelName);
    }

    public void LoadLevel(string levelName)
    {
        LevelSaver.LoadLevel(levelName, FindFirstObjectByType<GridManager>(), this);
    }
}
