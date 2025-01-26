using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    void Start()
    {
        int randomRotation = Random.Range(0, 4) * 90;
        transform.rotation = Quaternion.Euler(0, randomRotation, 0);
    }
}
