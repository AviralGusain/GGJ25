using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }
}
