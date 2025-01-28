using UnityEngine;

public class LevelStarCounter : MonoBehaviour
{
    public Transform StarPanel;
    public GameObject StarPrefab;
    public int StarCount = 0;
    public string LevelName = "Level1";

    private void Start()
    {
        PlayerScores scores = FindFirstObjectByType<PlayerScores>();
        if (scores != null && LevelName != null)
        {
            StarCount = scores.GetLevelScore(LevelName).numBubbles;
        }

        for (int i = 0; i < StarCount; i++)
        {
            Instantiate(StarPrefab, StarPanel);
        }
    }

    private void Update()
    {
        
    }
}
