using System.Runtime.CompilerServices;
using UnityEngine;

public class CounterDecor : MonoBehaviour
{
    public GameObject[] decorations;

    public int oneIn = 5;

    void Start()
    {
        int roll = Random.Range(0, oneIn);

        if(roll == 0)
        {
            Debug.Log("Got it");
            int number = Random.Range(0, decorations.Length);

            Instantiate(decorations[number], transform.position, Quaternion.identity);
        }
    }
}
