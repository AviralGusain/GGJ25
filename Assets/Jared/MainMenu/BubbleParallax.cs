using UnityEngine;

public class BubbleParallax : MonoBehaviour
{
    Vector3 StartPos;

    public int MoveModifier;

    private void Start()
    {
        StartPos = transform.position;
    }

    private void Update()
    {
        Vector2 pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (pz.x >= 0 && pz.x <= 1 && pz.y >= 0 && pz.y <= 1)
        {
            float PosX = Mathf.Lerp(transform.position.x, StartPos.x + (pz.x * MoveModifier), 2f * Time.deltaTime);
            float PosY = Mathf.Lerp(transform.position.y, StartPos.y + (pz.y * MoveModifier), 2f * Time.deltaTime);

            transform.position = new Vector3(PosX, PosY, StartPos.z);
        }
    }
}
