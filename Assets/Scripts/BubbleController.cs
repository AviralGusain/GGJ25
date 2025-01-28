using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class BubbleController : MonoBehaviour
{
  public Rigidbody bubble;
  private GameObject bouncer;
  private GameObject launcher;
  private GameObject windObject;

  private AudioSource[] audioSources;

  private float moveSpeed = 0.0f;

  private float defaultSpeed = 5f;
  private float speedMultiplier = 2.0f;

  public Vector3 direction = Vector3.zero;

  private bool lerp = false;
  private bool launching = false;
  private Vector3 finalPos;

  public Animator bubbleAnimator;
    public GameObject popParticles;

  private float distanceTraveled = 0.0f;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    bubbleAnimator = GetComponentInChildren<Animator>();
    moveSpeed = defaultSpeed;

    audioSources = GetComponents<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
  }

  private void FixedUpdate()
  {
    if (launching && !lerp)
    {
      if (launcher == null)
      {
        audioSources[0].Play();
        PopSequence();
        return;
      }

      LauncherController launchController = launcher.GetComponent<LauncherController>();
      bubble.gameObject.GetComponent<Collider>().enabled = false;

      // Function will return bool that determines if position was reached
      Vector3 newPos = launchController.LaunchBubble(bubble.gameObject, direction, moveSpeed, Time.deltaTime, ref launching, ref distanceTraveled);
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
    if (ReferenceEquals(windObject, collider.gameObject))
    {
      moveSpeed = defaultSpeed * speedMultiplier;
      gameObject.GetComponent<Collider>().enabled = true;
      return;
    }

    moveSpeed = defaultSpeed;

    // If object is untagged, destroy it
    if (collider.CompareTag("Undergrid") || collider.CompareTag("Spawner") || collider.CompareTag("Bubble"))
    {
      return;
    }

    gameObject.GetComponent<Collider>().enabled = false;

    // Collision with a bouncer, pass the bouncer controller to the bouncer collision method
    if (collider.TryGetComponent(out BouncerController bouncerController) && !launching)
    {
      BouncerCollision(bouncerController);
      return;
    }

    // Collision with a launcher
    if (collider.TryGetComponent(out LauncherController launchController))
    {
      audioSources[0].Play();

      // Play launch animation
      launchController.animator.SetTrigger("Launch");

      LauncherCollision(launchController);

      return;
    }

    // Collision with a wind object
    if (collider.CompareTag("Wind"))
    {
      windObject = collider.gameObject;
      WindCollision(collider.gameObject);
    }
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

    // reenable collider
    gameObject.GetComponent<Collider>().enabled = true;
  }

  void BouncerCollision(BouncerController bouncerController)
  {
    // Retrieve the gameobject attached to the bouncer
    bouncer = bouncerController.gameObject;

    bool possible = true;

    // Should reflect the bubble depending on orientation of the bouncer
    int plane = bouncerController.GetReflectionPlane(direction, ref possible);

    if (!possible)
    {
      PopSequence();
      return;
    }

    // Swap the x and z values of the direction vector
    GenericSwap<float>(ref direction.x, ref direction.z);

    if (plane % 2 != 0) direction *= -1;

    // Move in the direction by one tile using the bouncer transform
    finalPos = new Vector3(bouncer.transform.position.x + direction.x, bouncer.transform.position.y, bouncer.transform.position.z + direction.z);
    lerp = true;

    bubbleAnimator.SetTrigger("Bounce");

    StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
  }

  void WindCollision(GameObject wind)
  {
    // Play wind audio from
    AudioSource audio = wind.GetComponent<AudioSource>();
    if (audio != null)
    {
      audio.Play();
    }

    // Swap the x and z values of the direction vector
    direction = wind.transform.right;

    // Move in the direction by one tile using the fan transform
    finalPos = (direction.z == 0) ? new Vector3(bubble.transform.position.x + direction.x, wind.transform.position.y, wind.transform.position.z) : new Vector3(wind.transform.position.x, wind.transform.position.y, bubble.transform.position.z + direction.z);
    lerp = true;

    moveSpeed *= speedMultiplier;

    bubbleAnimator.SetTrigger("Bounce");

    // Start coroutine to move the bubble
    StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
  }

  void LauncherCollision(LauncherController launcherController)
  {
    launcher = launcherController.gameObject;

    // Swap the x and z values of the direction vector
    direction = launcher.transform.forward;

    // Move in the direction by one tile using the bouncer transform
    finalPos = launcher.transform.position;
    lerp = true;
    launching = true;

    bubbleAnimator.SetTrigger("Bounce");

    StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
  }

    public void PopSequence()
    {
        Instantiate(popParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}