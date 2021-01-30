using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyScroller : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted = true;

    public float _spawnTimer;

    public Transform _spawn1;
    public Transform _spawn2;
    public Transform _spawn3;

    private Vector3 _offset = new Vector3(0, 4, 0);

    private Coroutine _activeCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60f;

        _activeCoroutine = StartCoroutine(SpawnCoroutine());
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

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            int ran = Random.Range(1, 4);

            if (ran == 1)
            {
                SpawnEnemy(_spawn1.position + _offset);
            }

            else if (ran == 2)
            {
                SpawnEnemy(_spawn2.position + _offset);
            }

            else if (ran == 3)
            {
                SpawnEnemy(_spawn3.position+ _offset );
            }

            yield return new WaitForSeconds(_spawnTimer);
        }

        yield return null;
    }


    public void SpawnEnemy(Vector3 pos)
    {

        GameObject enemy = GameObject.Instantiate(CEnemyManager.Inst._enemyAsset, pos, Quaternion.identity);
        enemy.transform.parent = this.transform;

        CEnemyManager.Inst.addEnemy(enemy.GetComponent<CEnemy>());
    }
}
