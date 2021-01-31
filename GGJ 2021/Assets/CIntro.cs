using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIntro : MonoBehaviour
{
    private Animator _anim;


    private void Awake() 
    {
        _anim = GetComponent<Animator>();
    }

    public void Intro()
    {
        _anim.SetTrigger("Trigger");
    }
}
