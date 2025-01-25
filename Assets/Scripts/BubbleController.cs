using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed = 2f;
    public float idleTimeBeforePop = 2f;

    private Vector3 direction;
    private float idleTime;

    void Start()
    {
        direction = Vector3.right; // Initial direction
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (IsIdle())
        {
            idleTime += Time.deltaTime;
            if (idleTime > idleTimeBeforePop)
            {
                Pop();
            }
        }
        else
        {
            idleTime = 0;
        }
    }

    bool IsIdle()
    {
        return direction == Vector3.zero;
    }

    void Pop()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bouncer"))
        {
            direction = Quaternion.Euler(0, 0, 90) * direction;
        }
        else if (collision.CompareTag("Fan"))
        {
            direction *= 2; // Double speed for the effect
        }
        else if (collision.CompareTag("Edge"))
        {
            Pop();
        }
    }
}
