using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class BubbleController : MonoBehaviour
{
  public Rigidbody bubble;
  private GameObject bouncer;
  private GameObject launcher;

  private Vector3 input;
  private float moveSpeed = 0.0f;

  private float defaultSpeed = 5f;
  private float speedMultiplier = 2.0f;

  public Vector3 direction = Vector3.left;

  private bool freeMovement = false;

  private bool lerp = false;
  private bool launching = false;
  private Vector3 finalPos;

  public bool reset = false;

    public Animator bubbleAnimator;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
        bubbleAnimator = GetComponentInChildren<Animator>();
    moveSpeed = defaultSpeed;
  }

  // Update is called once per frame
  void Update()
  {
    input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


    // if (!freeMovement) BouncerTest(Time.deltaTime);

    // STRICTLY FOR TESTING PURPOSES
    if (reset)
    {
      bubble.transform.position = new Vector3(3, 1, 0);
      direction = Vector3.left;
      reset = false;
    }
  }

  private void FixedUpdate()
  {
    // REMOVE WHEN FINISHED
    if (freeMovement)
    {
      bubble.MovePosition(bubble.position + input * Time.deltaTime * moveSpeed);
    }

    if (launching && !lerp)
    {
      LauncherController launchController = launcher.GetComponent<LauncherController>();

      // Function will return bool that determines if position was reached
      Vector3 newPos = launchController.LaunchBubble(bubble.gameObject, direction, moveSpeed, Time.deltaTime, ref launching);
      bubble.MovePosition(newPos);

      return;
    }

    // Raycast to check if the bubble is in line with a fan
    else if (!lerp && !launching)
    {
      bubble.MovePosition(bubble.position + moveSpeed * Time.deltaTime * direction);
    }
  }

  private void GenericSwap<T>(ref T a, ref T b)
  {
    T temp = a;
    a = b;
    b = temp;
  }

  private void OnTriggerEnter(Collider collider)
  {
        moveSpeed = defaultSpeed;

    // Collision with a bouncer, pass the bouncer controller to the bouncer collision method
    if (collider.TryGetComponent(out BouncerController bouncerController) && !launching)
    {
      BouncerCollision(bouncerController);
    }

    // If colliding with a wind object, pass the wind object to the fan collision method
    if (collider.CompareTag("Wind"))
    {
      GameObject parent = collider.transform.parent.gameObject;
      Debug.Log("Parent tag: " + parent.tag);

      FanCollision(parent);
    }

    if (collider.TryGetComponent(out LauncherController launchController))
    {
      LauncherCollision(launchController);
    }

    //if (collider.CompareTag("Wall"))
    //{
    //    Debug.Log("Bubble should go poppy");
    //    Destroy(gameObject);
    //}

    //    if (collider.CompareTag("Fan"))
    //    {
    //        Debug.Log("Bubble should go poppy");
    //        Destroy(gameObject);
    //    }
    }

  IEnumerator MoveOverTime(Transform obj, Vector3 startPos, Vector3 endPos, float speed)
  {
    float duration = (moveSpeed > 0.0f) ? (Vector3.Distance(bubble.position, finalPos) / moveSpeed) : 0;
    float elapsedTime = 0;

    while (elapsedTime < duration)
    {
      obj.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
      elapsedTime += Time.deltaTime;
      yield return null;
    }

    obj.position = endPos; // Ensure it reaches the exact position
    lerp = false;
  }

  void BouncerCollision(BouncerController bouncerController)
  {
        
        //AUDIO

        // Retrieve the gameobject attached to the bouncer
        bouncer = bouncerController.gameObject;

    bool possible = true;

    // Should reflect the bubble depending on orientation of the bouncer
    int plane = bouncerController.GetReflectionPlane(direction, ref possible);

    if (!possible)
    {
      Destroy(gameObject);
      return;
    }


    Debug.Log("Plane: " + plane);

    // Swap the x and z values of the direction vector
    GenericSwap<float>(ref direction.x, ref direction.z);

    if (plane % 2 != 0) direction *= -1;

    // Move in the direction by one tile using the bouncer transform
    finalPos = new Vector3(bouncer.transform.position.x + direction.x, 1.0f, bouncer.transform.position.z + direction.z);
    lerp = true;

        // Calculate time it should take to move to the next tile

        //AUDIO
        bubbleAnimator.SetTrigger("Bounce");

        StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
  }

  void FanCollision(GameObject fan)
  {
    Debug.Log("Wind collision");

    // Swap the x and z values of the direction vector
    direction = fan.transform.right;

    // Move in the direction by one tile using the bouncer transform
    finalPos = (direction.z == 0) ? new Vector3(bubble.transform.position.x + direction.x, 1.0f, fan.transform.position.z) : new Vector3(fan.transform.position.x, 1.0f, bubble.transform.position.z + direction.z);
    lerp = true;

    Debug.Log(finalPos);

    moveSpeed *= speedMultiplier;

        // Calculate time it should take to move to the next tile
        //AUDIO
        bubbleAnimator.SetTrigger("Bounce");

        StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
  }

  void LauncherCollision(LauncherController launcherController)
  {
    Debug.Log("Launcher collision");

    launcher = launcherController.gameObject;

    // Swap the x and z values of the direction vector
    direction = launcher.transform.forward;

    // Move in the direction by one tile using the bouncer transform
    finalPos = launcher.transform.position;
    lerp = true;
    launching = true;

    Debug.Log("Launcher Position: " + finalPos);

        // Calculate time it should take to move to the next tile
        //AUDIO
        bubbleAnimator.SetTrigger("Bounce");

        StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
  }
}
