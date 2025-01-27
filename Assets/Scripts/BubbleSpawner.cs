using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;
    public int travelDistance = 3; // Default travel distance
    public bool limitTravelDistance = true; // Whether bubbles have a limited travel distance

    private float spawnTimer;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnBubble();
            spawnTimer = 0f;
        }
    }

    void SpawnBubble()
    {
        anim.SetTrigger("Spawn");

        GameObject bubble = Instantiate(bubblePrefab, spawnPoint.position, Quaternion.identity);

        // Configure the bubble's travel settings
        Bubble bubbleScript = bubble.GetComponent<Bubble>();
        if (bubbleScript != null)
        {
            bubbleScript.travelDistance = travelDistance;
            bubbleScript.limitTravelDistance = limitTravelDistance;
        }
    }
}
