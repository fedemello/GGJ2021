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
    }
}
