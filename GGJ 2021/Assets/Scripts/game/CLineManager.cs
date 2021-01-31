using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLineManager : MonoBehaviour
{
    public static CLineManager Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("Line Manager").AddComponent<CLineManager>();
            return _inst;
        }
    }
    private static CLineManager _inst;

    public List<CustomTile> _topLine = new List<CustomTile>();
    public List<CustomTile> _midLine = new List<CustomTile>();
    public List<CustomTile> _botLine = new List<CustomTile>();  

    void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;
        
        AllOf();
    }

    public void AllOf()
    {
        BottomLineOff();
        MidLineOff();
        TopLineOff();
    }

    public void BottomLineOff()
    {
        for (int i = 0; i < _botLine.Count; i++)
        {
            _botLine[i].Off();
        }
    }

    public void MidLineOff()
    {
        for (int i = 0; i < _midLine.Count; i++)
        {
            _midLine[i].Off();
        }
    }

    public void TopLineOff()
    {
        for (int i = 0; i < _topLine.Count; i++)
        {
            _topLine[i].Off();
        }
    }    
    public void BottomLineOn()
    {
        for (int i = 0; i < _botLine.Count; i++)
        {
            _botLine[i].On();
        }
    }

    public void MidLineOn()
    {
        for (int i = 0; i < _midLine.Count; i++)
        {
            _midLine[i].On();
        }
    }

    public void TopLineOn()
    {
        for (int i = 0; i < _topLine.Count; i++)
        {
            _topLine[i].On();
        }
    }
}
