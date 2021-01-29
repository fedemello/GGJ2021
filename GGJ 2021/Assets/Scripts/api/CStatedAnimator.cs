using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStatedAnimator : MonoBehaviour
{
    public Animator _anim;

    private int _state = 0;

    public void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void GoNextState()
    {
        if(_state <= 1)
        {
            _state += 1;
            _anim.SetInteger("State", _state);
        }
    }
}