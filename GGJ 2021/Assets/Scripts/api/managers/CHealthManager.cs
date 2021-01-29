using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CPlayer))]
public class CHealthManager : MonoBehaviour
{
    public int _maxHP = 100;
    public int _currentHP;

    public void Awake()
    {
        MaxHP();
    }

    public void MaxHP()
    {
        _currentHP = _maxHP;
    }

    public int GetHP()
    {
        return _currentHP;
    }

    public int GetMaxHP()
    {
        return _maxHP;
    }

    public float GetPercent()
    {
        return _currentHP / (float)_maxHP;
    }

    public void AddHP(int add)
    {
        _currentHP = Mathf.Clamp(_currentHP + add, 0, _maxHP);
    }

    public void LessHP(int less)
    {
        _currentHP = Mathf.Clamp(_currentHP - less, 0, _maxHP);
    }

    public void AddPercent(float percent)
    {
        percent = Mathf.Clamp(percent, 0, 100);
        AddHP(Mathf.FloorToInt(_maxHP * (percent / 100f)));
    }

    public void LessPercent(float percent)
    {
        percent = Mathf.Clamp(percent, 0, 100);
        LessHP(Mathf.FloorToInt(_maxHP * (percent / 100f)));
    }

    public bool HasHP()
    {
        return _currentHP > 0;
    }

    public void ZeroHP()
    {
        _currentHP = 0;
    }
}
