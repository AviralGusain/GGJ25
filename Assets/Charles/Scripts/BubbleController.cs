using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class BubbleController : MonoBehaviour
{
  public Rigidbody bubble;
  private GameObject bouncer;

  private Vector3 input;
  public float moveSpeed = 0.0f;

  public float defaultSpeed = 6f;
  public float speedMultiplier = 2.0f;

  public Vector3 direction = Vector3.left;

  public bool freeMovement = true;

  private bool lerp = false;
  private Vector3 finalPos;

  public bool reset = false;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
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
    if (freeMovement)
    {
      bubble.MovePosition(bubble.position + input * Time.deltaTime * moveSpeed);
    }
    else if (!lerp)
    {
      bubble.MovePosition(bubble.position + direction * Time.deltaTime * moveSpeed);
    }

    // Raycast to check if the bubble is in line with a fan
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
    if (collider.TryGetComponent(out BouncerController bouncerController) )
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
    StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
  }
}
