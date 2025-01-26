using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class BubbleController : MonoBehaviour
{
  public Rigidbody bubble;
  private GameObject bouncer;

  private Vector3 input;
  public float moveSpeed = 7.5f;

  public Vector3 direction = Vector3.left;

  public bool freeMovement = true;

  private bool lerp = false;
  private Vector3 finalPos;

  public bool reset = false;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

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

  private void OnTriggerEnter(Collider collision)
  {

    // Collision with a bouncer, pass the bouncer controller to the bouncer collision method
    if (collision.TryGetComponent(out BouncerController bouncerController) )
    {
      BouncerCollision(bouncerController);
    }

    // If colliding with a wind object, pass the wind object to the fan collision method
    if (collision.CompareTag("Wind"))
    {
      GameObject parent = collision.transform.parent.gameObject;
      Debug.Log("Parent tag: " + parent.tag);

      FanCollision(parent);
    }

    // Destroy the bubble if it collides with a fan
    if (collision.CompareTag("Fan"))
    {
      Destroy(gameObject);
    }

    //if (collision.TryGetComponent(out FanController fanController))
    //{
    //  // Collision with a fan, pass the fan controller to the fan collision method
    //  FanCollision(fanController);
    //}

    //if (collision.TryGetComponent(out ) && !lerp)
    //{
    //  // Should reflect the bubble depending on orientation of the wall
    //  int plane = wallController.GetReflectionPlane(direction);
    //  // Swap the x and z values of the direction vector
    //  GenericSwap<float>(ref direction.x, ref direction.z);
    //  if (plane % 2 != 0) direction *= -1;
    //  // Move in the direction by one tile using the wall transform
    //  finalPos = new Vector3(bubble.transform.position.x + direction.x, 1.0f, bubble.transform.position.z + direction.z);
    //  lerp = true;
    //  // Calculate time it should take to move to the next tile
    //  StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
    //}
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
    finalPos = /*(direction.z == 0) ?*/ new Vector3(bubble.transform.position.x + direction.x, 1.0f, fan.transform.position.z);
    lerp = true;

    Debug.Log(finalPos);

    // Calculate time it should take to move to the next tile
    StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
  }

  //private bool test1 = false;
  //private bool test1_2 = true;
  //private bool test2 = true;
  //private bool test2_2 = true;
  //private bool test3 = true;
  //private bool test3_2 = true;
  //private bool test4 = true;
  //private bool test4_2 = true;


  //private void BouncerTest(float dt)
  //{
  //  timer += dt;

  //  if (!test1)
  //  {
  //    // Test 1
  //    Test1(true);
  //    test1 = true;
  //  }

  //  if (timer > 2.0f && !test1_2)
  //  {
  //    // Test 1
  //    Test1(false);
  //    test1_2 = true;
  //  }

  //  if (timer > 4.0f && !test2)
  //  {
  //    // Test 2
  //    Test2(true);
  //    test2 = true;
  //  }
  //  if (timer > 6.0f && !test2_2)
  //  {
  //    // Test 1
  //    Test2(false);
  //    test2_2 = true;
  //  }

  //  if (timer > 8.0f && !test3)
  //  {
  //    // Test 2
  //    Test3(true);
  //    test3 = true;
  //  }
  //  if (timer > 10.0f && !test3_2)
  //  {
  //    // Test 1
  //    Test3(false);
  //    test3_2 = true;
  //  }

  //  if (timer > 12.0f && !test4)
  //  {
  //    // Test 2
  //    Test4(true);
  //    test4 = true;
  //  }
  //  if (timer > 14.0f && !test4_2)
  //  {
  //    // Test 1
  //    Test4(false);
  //    test4_2 = true;
  //  }
  //}

  //private void Test1(bool first)
  //{
  //  // Move bouncer to the center of the screen
  //  bouncer.transform.position = new Vector3(0, 1, 0);

  //  // Set the orientation of the bouncer to 0
  //  bouncer.transform.rotation = Quaternion.Euler(0, 0, 0);

  //  if (first)
  //  {
  //    // set the ball to the right of the bouncer
  //    bubble.transform.position = new Vector3(3, 1, 0);

  //    // Set the direction of the ball to the left
  //    direction = Vector3.left;
  //  }
  //  else
  //  {
  //    // set the ball to the left of the bouncer
  //    bubble.transform.position = new Vector3(0, 1, -5);

  //    // Set the direction of the ball to the right
  //    direction = Vector3.forward;
  //  }
  //}

  //private void Test2(bool first)
  //{
  //  // Set the orientation of the bouncer to 0
  //  bouncer.transform.rotation = Quaternion.Euler(0, 270, 0);

  //  if (first)
  //  {
  //    // set the ball to the right of the bouncer
  //    bubble.transform.position = new Vector3(5, 1, 0);

  //    // Set the direction of the ball to the left
  //    direction = Vector3.left;
  //  }
  //  else
  //  {
  //    // set the ball to the left of the bouncer
  //    bubble.transform.position = new Vector3(0, 1, 5);

  //    // Set the direction of the ball to the right
  //    direction = Vector3.back;
  //  }
  //}

  //private void Test3(bool first)
  //{
  //  // Set the orientation of the bouncer to 0
  //  bouncer.transform.rotation = Quaternion.Euler(0, 180, 0);

  //  if (first)
  //  {
  //    // set the ball to the right of the bouncer
  //    bubble.transform.position = new Vector3(0, 1, 5);

  //    // Set the direction of the ball to the left
  //    direction = Vector3.back;
  //  }
  //  else
  //  {
  //    // set the ball to the left of the bouncer
  //    bubble.transform.position = new Vector3(-5, 1, 0);

  //    // Set the direction of the ball to the right
  //    direction = Vector3.right;
  //  }
  //}

  //private void Test4(bool first)
  //{
  //  // Set the orientation of the bouncer to 0
  //  bouncer.transform.rotation = Quaternion.Euler(0, 90, 0);

  //  if (first)
  //  {
  //    // set the ball to the right of the bouncer
  //    bubble.transform.position = new Vector3(-5, 1, 0);

  //    // Set the direction of the ball to the left
  //    direction = Vector3.right;
  //  }
  //  else
  //  {
  //    // set the ball to the left of the bouncer
  //    bubble.transform.position = new Vector3(0, 1, -5);

  //    // Set the direction of the ball to the right
  //    direction = Vector3.forward;
  //  }
  //}
}
