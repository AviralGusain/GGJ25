using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;  // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake

    private Vector3 originalPosition;
    private float shakeTimeRemaining;

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * shakeMagnitude);
            shakeTimeRemaining -= Time.deltaTime;

            if (shakeTimeRemaining <= 0)
                transform.position = originalPosition;
        }
    }

    public void TriggerShake()
    {
        originalPosition = transform.position;
        shakeTimeRemaining = shakeDuration;
    }
}
