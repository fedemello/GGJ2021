using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COutro : MonoBehaviour
{
    private Animator _anim;

    private void Awake() 
    {
        _anim = GetComponent<Animator>();    
    }

    public void Credits()
    {
        _anim.SetTrigger("Credits");
    }
}
