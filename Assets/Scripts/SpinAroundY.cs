using UnityEngine;

public class SpinAroundY : MonoBehaviour
{
    // Speed of rotation in degrees per second
    [SerializeField] private float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the GameObject around its Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
