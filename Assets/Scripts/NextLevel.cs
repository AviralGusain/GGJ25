using UnityEngine;
using UnityEngine.Audio;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;
    public string lastLevel;

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
}
