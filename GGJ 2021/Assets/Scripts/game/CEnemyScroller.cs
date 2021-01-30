﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyScroller : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted;

    private float alturaFila1 = 0.41f;
    private float alturaFila2 = -1.5f;
    private float alturaFila3 = -3.45f;


    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60f;

        this.SpawnEnemy(new Vector3(0, alturaFila2, 0));
        this.SpawnEnemy(new Vector3(5, alturaFila1, 0));
        this.SpawnEnemy(new Vector3(10, alturaFila3, 0));
        this.SpawnEnemy(new Vector3(15, alturaFila3, 0));
        this.SpawnEnemy(new Vector3(20, alturaFila1, 0));
        this.SpawnEnemy(new Vector3(25, alturaFila2, 0));




    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
            }
        }
        else
        {
            // Este movimiento se aplica a todos los enemigos simultaneamente
            transform.position -= new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        }


    }


    public void SpawnEnemy(Vector3 pos)
    {
        // Spawnea a un enemigo en la ubicacíon pasasda por paramentro
        // Y se agrega al manager de enemigos

        GameObject enemy = GameObject.Instantiate(CEnemyManager.Inst._enemyAsset, pos, Quaternion.identity);
        enemy.transform.parent = this.transform;
        if(pos.y == alturaFila1)
        {
            enemy.GetComponent<CEnemy>().line = 1;
        }
        else if (pos.y == alturaFila2)
        {
            enemy.GetComponent<CEnemy>().line = 2;
        }
        else if (pos.y == alturaFila3)
        {
            enemy.GetComponent<CEnemy>().line = 3;
        }

        CEnemyManager.Inst.addEnemy(enemy.GetComponent<CEnemy>());

    }
}
