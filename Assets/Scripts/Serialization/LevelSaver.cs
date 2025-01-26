using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Playables;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

[System.Serializable]
public struct LevelItemPackage
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;
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

    public static void SaveCurrentLevel(GridManager grid)
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

        // Make a list of all objects
        grid.SetLevelSaveData(levelItemsList);

        //Debug.Log(grid.levelSaveDataString);

        // Convert grid class to json
        string gridJson = JsonUtility.ToJson(grid, true);

        // Write json to file
        StreamWriter writer = new StreamWriter("TestLevel.json", false);
        writer.WriteLine(gridJson);
        writer.Close();
    }

    // Loads level into the given grid and level state managers
    public static void LoadLevel(string levelName, GridManager grid, LevelStateManager levelState)
    {
        grid.ResetGridToEmpty();

        grid.RebuildGrid();

        string levelJson = File.ReadAllText(levelName + ".json");

        JsonUtility.FromJsonOverwrite(levelJson, grid); // Load in grid data

        List<LevelItemPackage> levelObjects = grid.GetLevelSaveData(); // load in data for level objects to spawn

        // Spawn each saved object
        foreach (LevelItemPackage levelObjectData in levelObjects)
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
