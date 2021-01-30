using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyManager : MonoBehaviour
{

    public static CEnemyManager Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("Enemy Manager").AddComponent<CEnemyManager>();
            return _inst;
        }
    }

    private static CEnemyManager _inst;

    public GameObject _enemyAsset;
    private List<CEnemy> _enemies = new List<CEnemy>();

    public CEnemy _firstEnemy;

    private void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;

    }

    public void addEnemy(CEnemy newEnemy)
    {
        _enemies.Add(newEnemy);

        if (_firstEnemy == null)
        {
            _firstEnemy = newEnemy;
        }
    }

    private void Update() 
    {
        //if (_firstEnemy == null)
        //    UpdateFirstEnemy();
    }

    private void UpdateFirstEnemy()
    {
        for (int i = 0; i < _enemies.Count; i ++)
        {
            if (_firstEnemy == null)
            {
                _firstEnemy = _enemies[i];
            }
            else
            {
                if (_enemies[i].transform.position.x < _firstEnemy.transform.position.x)
                {
                    _firstEnemy = _enemies[i];
                }
            }
        }

        Debug.Log("updating first enemy: " + _firstEnemy);
    }

    public void ImOut(CEnemy it)
    {
        _enemies.Remove(it);

        if (it == _firstEnemy)
        {
            _firstEnemy = null;

            UpdateFirstEnemy();
        }
    }

    public CEnemy FirstEnemy()
    {
        return _firstEnemy;
    }
}
