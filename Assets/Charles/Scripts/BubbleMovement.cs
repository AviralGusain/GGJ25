using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
  public Rigidbody bubble;
  public Vector3 input;
  public float moveSpeed = 10f;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
  }

  private void FixedUpdate()
  {
    bubble.MovePosition(bubble.position + input * Time.deltaTime * moveSpeed);
  }
}
