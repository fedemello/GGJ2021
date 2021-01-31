using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSinger : MonoBehaviour
{
    private Animator _anim;

    private void Awake() 
    {
        _anim = GetComponent<Animator>();
        Off();
    }

    public void On()
    {
        _anim.SetBool("On", true);
    }

    public void Off()
    {
        _anim.SetBool("On", false);
    }
}
