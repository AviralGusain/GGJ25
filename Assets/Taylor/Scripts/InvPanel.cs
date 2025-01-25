using UnityEditor.Animations;
using UnityEngine;

public class InvPanel : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PointerEnter()
    {
        anim.SetBool("Down", true);
    }

    public void PointerExit()
    {
        anim.SetBool("Down", false);
    }
}
