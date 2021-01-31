using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTile : MonoBehaviour
{
    public SpriteRenderer _spr;
    public Sprite _spriteOn;
    public Sprite _spriteOff;

    public void Awake() 
    {
        _spr = GetComponent<SpriteRenderer>();
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
