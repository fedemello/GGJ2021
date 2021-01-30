using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyScroller : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted;

    public Transform _spawn1;
    public Transform _spawn2;
    public Transform _spawn3;

    private Vector3 _offset = new Vector3(0, 4, 0);


    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60f;

        this.SpawnEnemy(_spawn1.position + _offset);
        this.SpawnEnemy(_spawn2.position + _offset);
        this.SpawnEnemy(_spawn3.position + _offset);
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
            transform.position -= new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        }
    }


    public void SpawnEnemy(Vector3 pos)
    {

        GameObject enemy = GameObject.Instantiate(CEnemyManager.Inst._enemyAsset, pos, Quaternion.identity);
        enemy.transform.parent = this.transform;

        CEnemyManager.Inst.addEnemy(enemy.GetComponent<CEnemy>());

    }
}
