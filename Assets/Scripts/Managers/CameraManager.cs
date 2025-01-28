using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraTransform;
    private bool perspective = true;

    public Transform perspectivePosition;
    public Transform topDownPosition;

    private Transform currentTarget;
    public float transitionSpeed;

    private float elapsedTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //cameraTransform = Camera.main.transform.parent;
        currentTarget = perspectivePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            perspective = !perspective;
            currentTarget = perspective ? perspectivePosition : topDownPosition;

            elapsedTime = 0;
        }

        AdjustCamera();
    }

    public void Swap()
    {
        perspective = !perspective;
        currentTarget = perspective ? perspectivePosition : topDownPosition;

        elapsedTime = 0;
    }

    private void AdjustCamera()
    {
        float duration = (transitionSpeed > 0.0f) ? (Vector3.Distance(cameraTransform.position, currentTarget.position) / transitionSpeed) : 0;

        if(elapsedTime < duration)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, currentTarget.position, elapsedTime / duration);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, currentTarget.rotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
        }
    }
}
