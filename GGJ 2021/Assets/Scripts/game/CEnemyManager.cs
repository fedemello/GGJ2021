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

    public AudioClip _enemyAudio;

    public float _enemyVolume;

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

            if (_firstEnemy._line == 2 && _firstEnemy._state == CEnemy._STATE_ON)
            {
                playEnemySfx();
            }
        }
    }

    private void Update()
    {
        //if (_firstEnemy == null)
        //    UpdateFirstEnemy();
    }

    private void UpdateFirstEnemy()
    {
        for (int i = 0; i < _enemies.Count; i++)
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

        if (_firstEnemy != null)
        {
            if (_firstEnemy._line == 2 && _firstEnemy._state == CEnemy._STATE_ON)
            {
                playEnemySfx();
            }
        }
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
    public void playEnemySfx()
    {
        if (!CAudioManager.Inst.isSfxPlaying("enemy_audio"))
        {
            CAudioManager.Inst.playSfx("enemy_audio", _enemyAudio, _enemyVolume);
        }
    }

    public void stopEnemySfx()
    {
        if (CAudioManager.Inst.isSfxPlaying("enemy_audio"))
        {
            CAudioManager.Inst.stopSfx("enemy_audio");
        }
    }

    public int cantEnemies()
    {
        return _enemies.Count;
    }

    public void clearEnemies()
    {
        for (int i = _enemies.Count- 1; i >= 0; i--)
        {
            _enemies[i].Killed();
            _enemies.RemoveAt(i);
        }
    }
}
