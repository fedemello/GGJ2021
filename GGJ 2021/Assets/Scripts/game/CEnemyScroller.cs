using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyScroller : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted = true;

    public float _spawnMinTimer;
    public float _spawnMaxTimer;


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
            // Este movimiento se aplica a todos los enemigos simultaneamente
            //transform.position -= new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            int ran = Random.Range(1, 4);

            if (ran == 1)
            {
                SpawnEnemy(_spawn1.position + _offset, 1);

            }
            else if (ran == 2)
            {
                SpawnEnemy(_spawn2.position + _offset, 2);

            }
            else if (ran == 3)
            {
                SpawnEnemy(_spawn3.position+ _offset, 3);
            }

            // Hack para convocar solo en fila 3 comentando los if de ran. 
            //SpawnEnemy(_spawn3.position + _offset, 3);

            yield return new WaitForSeconds(Random.Range(_spawnMinTimer, _spawnMaxTimer));
        }

        yield return null;
    }

    public void SpawnEnemy(Vector3 pos, int line)
    {
        // Spawnea a un enemigo en la ubicacíon pasasda por paramentro
        // Y se agrega al manager de enemigos

        int pitch = Random.Range(0, 3);

        float posYOriginal = pos.y;

        pos = new Vector3(pos.x, pos.y + beatTempo + 30, pos.x);

        GameObject enemy = GameObject.Instantiate(CEnemyManager.Inst._enemyAsset, pos, Quaternion.identity);
        enemy.transform.parent = this.transform;
        
        
        CEnemy enemyccc = enemy.GetComponent<CEnemy>();
        enemyccc.beatTempo = beatTempo;

        enemyccc._line = line;
        enemyccc._pitch = pitch;
        enemyccc._lineY = posYOriginal;

        CEnemyManager.Inst.addEnemy(enemy.GetComponent<CEnemy>());
    }
}
