using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;
    public string lastLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
