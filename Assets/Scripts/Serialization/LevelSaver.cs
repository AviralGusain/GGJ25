using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Overlays;
using UnityEditor.Playables;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using static FileHandler;

[System.Serializable]
public struct LevelItemPackage
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class LevelStatData
{
    public int numStartingBouncers = 5;
    public int numStartingFans = 5;
    public int numStartingLaunchers = 5;

    public ArraySerializeWrapper<int> mBubbleScoreTargets = new ArraySerializeWrapper<int>();

    public ArraySerializeWrapper<LevelItemPackage> mLevelObjects = new ArraySerializeWrapper<LevelItemPackage>();
}



public class FileHandler
{
    public static T[] LoadFromJson<T> (string json)
    {
        ArraySerializeWrapper<T> wrapper = JsonUtility.FromJson<ArraySerializeWrapper<T>>(json);
        return wrapper.mItems;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        ArraySerializeWrapper<T> wrapper = new ArraySerializeWrapper<T>();
        wrapper.mItems = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    public class ArraySerializeWrapper<T>
    {
        public T[] mItems;
    }
}

[CreateAssetMenu(fileName = "LevelSaver", menuName = "Scriptable Objects/LevelSaver")]

public class LevelSaver : ScriptableObject
{

    public static void SaveCurrentLevel(GridManager grid, LevelStateManager levelManager)
    {
        // scratch variables for looping through items
        LevelItemPackage currItem = new LevelItemPackage();
        GameObject currLevelObject = null;

        List<LevelItemPackage> levelItemsList = new List<LevelItemPackage>(); // List to make of current objects

        // for each object in the grid
        for (int i = 0; i < grid.gridWidth; i++)
        {
            for (int j = 0; j < grid.gridHeight; j++)
            {
                currLevelObject = grid.GetLevelGridItem(i, j);

                if (currLevelObject == null) // if no object here
                {
                    continue; // Skip tile
                }

                currItem.prefabName = "";

                // Set prefab name for getting the correct prefab when spawning it
                if (currLevelObject.CompareTag("Bouncer"))
                {
                    currItem.prefabName = "Bouncer";
                }
                else if (currLevelObject.CompareTag("Fan"))
                {
                    currItem.prefabName = "Fan";
                }
                else if (currLevelObject.CompareTag("Launcher"))
                {
                    currItem.prefabName = "Launcher";
                }
                else if (currLevelObject.CompareTag("BaseGoal"))
                {
                    currItem.prefabName = "BaseGoal";
                }



                // Save position
                currItem.position = currLevelObject.transform.position;

                // Save rotation
                currItem.rotation = currLevelObject.transform.rotation;

                levelItemsList.Add(currItem); // Add to list of saved items
            }
        }

        
        LevelStatData saveData = new LevelStatData();
        // Save starting resources
        saveData.numStartingBouncers = levelManager.mNumStartingBouncers;
        saveData.numStartingFans = levelManager.mNumStartingFans;
        saveData.numStartingLaunchers = levelManager.mNumStartingLaunchers;

        // Save goal types
        saveData.mBubbleScoreTargets.mItems = levelManager.mBubbleScoreTargets.ToArray();

        saveData.mLevelObjects.mItems = levelItemsList.ToArray();


        // Make a list of all objects
        //grid.SetLevelSaveData(levelItemsList);

        Debug.Log("LevelSaver:SaveLevel: Level saved as \"Test Level2\"");

        // Convert grid class to json
        //string gridJson = JsonUtility.ToJson(grid, true);
        string levelSaveJson = JsonUtility.ToJson(saveData, true);

        // Write json to file
        StreamWriter writer = new StreamWriter("TestLevel2.json", false);
        writer.WriteLine(levelSaveJson);
        writer.Close();
    }

    // Loads level into the given grid and level state managers
    public static void LoadLevel(string levelName, GridManager grid, LevelStateManager levelState)
    {
        grid.ResetGridToEmpty();

        grid.RebuildGrid();

        

        string levelJson = File.ReadAllText(levelName + ".json");



        //GridManager tempGridManager = JsonUtility.FromJson<GridManager>(levelJson);
        //JsonUtility.FromJsonOverwrite(levelJson, grid); // Load in grid data
        LevelStatData loadedData = new LevelStatData();
        JsonUtility.FromJsonOverwrite(levelJson, loadedData); // Load in level grid data

        //ArraySerializeWrapper<LevelItemPackage> testList = new ArraySerializeWrapper<LevelItemPackage>();
        //JsonUtility.FromJsonOverwrite(levelJson, testList); // Load in grid data


        //List<LevelItemPackage> levelObjects = grid.GetLevelSaveData(); // load in data for level objects to spawn
        //List<LevelItemPackage> levelObjects = grid.GetLevelSaveData(); // load in data for level objects to spawn

        // Load in level stats
        levelState.mBubbleScoreTargets = new List<int>(loadedData.mBubbleScoreTargets.mItems); // Load in score targets

        // Load in resources
        levelState.mNumStartingBouncers = loadedData.numStartingBouncers;
        levelState.mNumStartingFans = loadedData.numStartingFans;
        levelState.mNumStartingLaunchers = loadedData.numStartingLaunchers;

        // Set inventory based on loaded info (make sure to use set function)
        FindFirstObjectByType<Inventory>().SetNumBouncers(levelState.mNumStartingBouncers);
        FindFirstObjectByType<Inventory>().SetNumFans(levelState.mNumStartingFans);

        // Spawn each saved object
        foreach (LevelItemPackage levelObjectData in loadedData.mLevelObjects.mItems)
        {
            GameObject prefabToUse = grid.GetPrefabByTagName(levelObjectData.prefabName);
            if (prefabToUse == null)
            {
                Debug.Log("LevelSaver:LoadLevel: no matching prefab of name " + levelObjectData.prefabName + " to use, check gridmanager prefab getter if this seems wrong.");
                continue;
            }
            GameObject placedObject = grid.PlaceObjectAtPosition(levelObjectData.position, prefabToUse); // place gameobject, with saved position, in levek
            placedObject.transform.rotation = levelObjectData.rotation; // Set rotation
        }
    }

    //public static string FixGeneratedJSON(string rawJson)
    //{
    //    rawJson = "{\"Items:\":" + rawJson + "}";
    //    return rawJson;
    //}

}
