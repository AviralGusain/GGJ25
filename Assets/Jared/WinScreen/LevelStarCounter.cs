using UnityEngine;

public class LevelStarCounter : MonoBehaviour
{
    public Transform StarPanel;
    public GameObject StarPrefab;
    public int StarCount = 0;

    private void Start()
    {
        for (int i = 0; i < StarCount; i++)
        {
            Instantiate(StarPrefab, StarPanel);
        }
    }

    private void Update()
    {
        
    }
}
