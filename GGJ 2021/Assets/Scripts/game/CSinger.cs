using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSinger : MonoBehaviour
{
    private SpriteRenderer _spr;

    public Sprite _spriteOn;
    public Sprite _spriteOff;

    private void Awake() 
    {
        _spr = GetComponent<SpriteRenderer>();
        Off();
    }

    public void On()
    {
        _spr.sprite = _spriteOn;
    }
    public void Off()
    {
        _spr.sprite = _spriteOff;
    }
}
