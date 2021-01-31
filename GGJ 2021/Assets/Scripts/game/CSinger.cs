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

    public void Azul()
    {
        _anim.SetTrigger("Azul");
    }
    
    public void Amarillo()
    {
        _anim.SetTrigger("Amarillo");
    }

    public void GuitarOn()
    {
        _anim.SetBool("GuitarOn", true);
    }

    public void GuitarOff()
    {
        _anim.SetBool("GuitarOn", false);
    }
}
