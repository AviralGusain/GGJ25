using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CounterDecor : MonoBehaviour
{
    public GameObject[] decorations;

    public int oneIn = 5;

    public Transform decorParent;

    void Start()
    {
        int roll = Random.Range(0, oneIn);

        if(roll == 0)
        {
            int randomThree = Random.Range(0, 3);

            GameObject obj = Instantiate(decorations[randomThree], decorParent.position, Quaternion.identity);
            obj.transform.SetParent(decorParent);

            //for (int i = 0; i <= randomThree; i++)
            //{
            //    Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            //    GameObject obj = Instantiate(decorations[i], offset, Quaternion.identity);
            //    obj.transform.SetParent(decorParent);
            //}

            int randomRotation = Random.Range(0, 4) * 90;
            decorParent.rotation = Quaternion.Euler(0, randomRotation, 0);
        }
    }
}
