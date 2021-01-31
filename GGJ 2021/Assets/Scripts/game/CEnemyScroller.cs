using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyScroller : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted = true;

    public float _spawnMinTimer;
    public float _spawnMaxTimer;

    public int _cantidadMaxSpawn = 20;
    public int _currentCantidadSpawn = 0;
    public float _porcentajeParaSobrevivir = 50f;

    public float _valorParaBarra;

    private bool _winCondition = false;
    private bool _loseCondition = false;

    private int _puntajeMaximo;

    private int maxScore;

    public Transform _spawn1;
    public Transform _spawn2;
    public Transform _spawn3;

    private Vector3 _offset = new Vector3(-40, 4, 0);

    private Coroutine _activeCoroutine;

    public CPlayerLifebar _playerScore;


    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60f;

        _puntajeMaximo = CScoreManager.Inst.getMaxScore();

        _playerScore.setCurrentPercent(0.5f);

        maxScore = _cantidadMaxSpawn * 50;
    }

    public void Spawn()
    {
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

        float puntaje = ((float)CScoreManager.Inst._score) / 50;
        float cantEnemigosVivos = (float)CEnemyManager.Inst.cantEnemies();

        float enemigosTotales = cantEnemigosVivos + (_cantidadMaxSpawn - _currentCantidadSpawn) * 50;

        float minimunScoreToWin = ((float)_cantidadMaxSpawn * 50) / 2;

        //Debug.Log("Puntaje: " + punatje);
        //Debug.Log("Cantidad Eniemigos Vivos: " + cantEnemigosVivos);
        //Debug.Log("Puntos faltantes: " + pointsRemaining);
        //Debug.Log("Minimo puntaje para ganar: " + minimunScoreToWin);

        _valorParaBarra = (float)(puntaje + enemigosTotales - minimunScoreToWin) / (float)(_cantidadMaxSpawn * 50);


        // Min que es minimunSCoreToWin    max que es (_cantidadMaxSpawn * 50);
        float value = puntaje + enemigosTotales; //Posible

        _valorParaBarra = CMath.lerp(minimunScoreToWin, maxScore, 0, 1, value);

        Debug.Log("valor: " + _valorParaBarra);

        //if (_valorParaBarra <= 0)
        //{
        //    _valorParaBarra = 0f;
        //}
        //else if (_valorParaBarra >= 1)
        //{
        //    _valorParaBarra = 1f;
        //}

        Debug.Log("Barra: " + _valorParaBarra);
        _playerScore.setCurrentPercent(_valorParaBarra);






        if ((puntaje + enemigosTotales - minimunScoreToWin) <= 0)
        {
            _loseCondition = true;
            Debug.Log("LOSE");
        }
        if (puntaje >= minimunScoreToWin)
        {
            _winCondition = true;
            Debug.Log("WIN");
        }





    }

    private IEnumerator SpawnCoroutine()
    {
        bool spawning = true;

        while (spawning)
        {
            CStateBase state = CLevelManager.Inst.getCurrentState();

            if (state != null && state.getState() == CSingingStage.STATE_ENDING)
            {
                spawning = false;
            }

            int ran = Random.Range(1, 4);

            if (ran == 1)
            {
                SpawnEnemy(_spawn1.position + _offset, 1, 45);

            }
            else if (ran == 2)
            {
                SpawnEnemy(_spawn2.position + _offset, 2, 47);

            }
            else if (ran == 3)
            {
                SpawnEnemy(_spawn3.position + _offset, 3, 49);
            }

            _currentCantidadSpawn = _currentCantidadSpawn + 1;

            if (_currentCantidadSpawn >= _cantidadMaxSpawn)
                spawning = false;

            // Hack para convocar en una sola fila comentando los if de ran. 
            //SpawnEnemy(_spawn2.position + _offset, 2);

            yield return new WaitForSeconds(Random.Range(_spawnMinTimer, _spawnMaxTimer));
        }

        yield return null;
    }

    public void SpawnEnemy(Vector3 pos, int line, int sorting)
    {
        // Spawnea a un enemigo en la ubicacíon pasasda por paramentro
        // Y se agrega al manager de enemigos

        int pitch = Random.Range(0, 3);

        float posYOriginal = pos.y;

        pos = new Vector3(pos.x, pos.y + beatTempo + 30, pos.z);

        GameObject enemy = GameObject.Instantiate(CEnemyManager.Inst._enemyAsset, pos, Quaternion.identity);
        enemy.transform.parent = this.transform;
        enemy.GetComponent<SpriteRenderer>().sortingOrder = sorting;

        CEnemy enemyccc = enemy.GetComponent<CEnemy>();
        enemyccc.beatTempo = beatTempo;

        enemyccc._line = line;
        enemyccc._pitch = pitch;
        enemyccc._lineY = posYOriginal;

        CEnemyManager.Inst.addEnemy(enemy.GetComponent<CEnemy>());
    }
}
