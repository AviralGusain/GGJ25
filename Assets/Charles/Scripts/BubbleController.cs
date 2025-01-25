using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class BubbleController : MonoBehaviour
{
  public Rigidbody bubble;
  private GameObject bouncer;

  public Vector3 input;
  public float moveSpeed = 7.5f;

  public Vector3 direction = Vector3.left;

  public bool freeMovement = true;

  public float timer = 0.0f;

  private bool lerp = false;
  private Vector3 finalPos;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


    if (!freeMovement) BouncerTest(Time.deltaTime);
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
    else 
    {
    }
  }

  private void GenericSwap<T>(ref T a, ref T b)
  {
    T temp = a;
    a = b;
    b = temp;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (collision.TryGetComponent(out BouncerController bouncerController) && !lerp)
    {
      // Retrieve the gameobject attached to the bouncer
      bouncer = bouncerController.gameObject;

      // Should reflect the bubble depending on orientation of the bouncer
      int plane = bouncerController.GetReflectionPlane(direction);

      Debug.Log("Plane: " + plane);

      // Swap the x and z values of the direction vector
      GenericSwap<float>(ref direction.x, ref direction.z);

      if (plane % 2 != 0) direction *= -1;

      Debug.Log("New Direction: " + direction);

      // Move in the direction by one tile using the bouncer transform
      finalPos = new Vector3(bouncer.transform.position.x + direction.x, 1.0f, bouncer.transform.position.z + direction.z);
      lerp = true;

      // Calculate time it should take to move to the next tile
      StartCoroutine(MoveOverTime(bubble.transform, bubble.position, finalPos, moveSpeed));
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

  private bool test1 = false;
  private bool test1_2 = true;
  private bool test2 = true;
  private bool test2_2 = true;
  private bool test3 = true;
  private bool test3_2 = true;
  private bool test4 = true;
  private bool test4_2 = true;


  private void BouncerTest(float dt)
  {
    timer += dt;

    if (!test1)
    {
      // Test 1
      Test1(true);
      test1 = true;
    }

    if (timer > 2.0f && !test1_2)
    {
      // Test 1
      Test1(false);
      test1_2 = true;
    }

    if (timer > 4.0f && !test2)
    {
      // Test 2
      Test2(true);
      test2 = true;
    }
    if (timer > 6.0f && !test2_2)
    {
      // Test 1
      Test2(false);
      test2_2 = true;
    }

    if (timer > 8.0f && !test3)
    {
      // Test 2
      Test3(true);
      test3 = true;
    }
    if (timer > 10.0f && !test3_2)
    {
      // Test 1
      Test3(false);
      test3_2 = true;
    }

    if (timer > 12.0f && !test4)
    {
      // Test 2
      Test4(true);
      test4 = true;
    }
    if (timer > 14.0f && !test4_2)
    {
      // Test 1
      Test4(false);
      test4_2 = true;
    }
  }

  private void Test1(bool first)
  {
    // Move bouncer to the center of the screen
    bouncer.transform.position = new Vector3(0, 1, 0);

    // Set the orientation of the bouncer to 0
    bouncer.transform.rotation = Quaternion.Euler(0, 0, 0);

    if (first)
    {
      // set the ball to the right of the bouncer
      bubble.transform.position = new Vector3(3, 1, 0);

      // Set the direction of the ball to the left
      direction = Vector3.left;
    }
    else
    {
      // set the ball to the left of the bouncer
      bubble.transform.position = new Vector3(0, 1, -5);

      // Set the direction of the ball to the right
      direction = Vector3.forward;
    }
  }

  private void Test2(bool first)
  {
    // Set the orientation of the bouncer to 0
    bouncer.transform.rotation = Quaternion.Euler(0, 270, 0);

    if (first)
    {
      // set the ball to the right of the bouncer
      bubble.transform.position = new Vector3(5, 1, 0);

      // Set the direction of the ball to the left
      direction = Vector3.left;
    }
    else
    {
      // set the ball to the left of the bouncer
      bubble.transform.position = new Vector3(0, 1, 5);

      // Set the direction of the ball to the right
      direction = Vector3.back;
    }
  }

  private void Test3(bool first)
  {
    // Set the orientation of the bouncer to 0
    bouncer.transform.rotation = Quaternion.Euler(0, 180, 0);

    if (first)
    {
      // set the ball to the right of the bouncer
      bubble.transform.position = new Vector3(0, 1, 5);

      // Set the direction of the ball to the left
      direction = Vector3.back;
    }
    else
    {
      // set the ball to the left of the bouncer
      bubble.transform.position = new Vector3(-5, 1, 0);

      // Set the direction of the ball to the right
      direction = Vector3.right;
    }
  }

  private void Test4(bool first)
  {
    // Set the orientation of the bouncer to 0
    bouncer.transform.rotation = Quaternion.Euler(0, 90, 0);

    if (first)
    {
      // set the ball to the right of the bouncer
      bubble.transform.position = new Vector3(-5, 1, 0);

      // Set the direction of the ball to the left
      direction = Vector3.right;
    }
    else
    {
      // set the ball to the left of the bouncer
      bubble.transform.position = new Vector3(0, 1, -5);

      // Set the direction of the ball to the right
      direction = Vector3.forward;
    }
  }
}
