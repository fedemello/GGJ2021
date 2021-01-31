using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemy : MonoBehaviour, ITriggered
{
    public static int _STATE_OFF = 0;
    public static int _STATE_ON = 1;

    public static int _STATE_ON_STAIRS = 0;
    public static int _STATE_OFF_STAIRS = 1;


    public Sprite _secondPhaseSprite;

    public int _line;
    public int _pitch;

    private bool _leftEye;
    private bool _middleEye;
    private bool _rightEye;

    private int mCurrentEye = 1;

    private bool _rightStick = false;

    private int _pace = 0; // de momento null

    public float _lineY = 0f;

    public int _triggersPassed = 0;

    public bool canBePressed;

    public bool inputTriggered;

    public float beatTempo = 0f;

    public int _state;
    private int _movement_state;

    private int currentHealth = 100;

    private Coroutine _activeCoroutine;
    
    private void Awake() 
    {
        _leftEye = (Random.value > 0.5f);
        _middleEye = (Random.value > 0.5f);
        _rightEye = (Random.value > 0.5f);

        float right = Random.value;

        if (right > 0.5f)
        {
            _rightStick = true;
        }
        else
        {
            _rightStick = false;
        }
    }
    
    private void Start() 
    {
        SetState(_STATE_OFF);
        SetMovementState(_STATE_ON_STAIRS);
    }


    // Update is called once per frame
    void Update()
    {
        if (_movement_state == _STATE_ON_STAIRS)
        {
            transform.position -= new Vector3(beatTempo * Time.deltaTime, beatTempo * Time.deltaTime, 0f);
        }
        else
        {
            transform.position -= new Vector3(beatTempo * Time.deltaTime, 0f, 0f);
        }

        CheckOffStair();



        if (_triggersPassed == 1)
        {
            SetState(_STATE_ON);
        }

        //Debug.Log(_state);
        //Debug.Log(_triggersPassed);

        if (_state == _STATE_ON)
        {
            if (_triggersPassed > 1)
            {
                _activeCoroutine = StartCoroutine(DestroyBySelfCoroutine());
            }
        }

        //inputTriggered = Input.GetKeyDown(KeyCode.F);

        // inputTriggered viene True cuando se da el input que le corresponde, canBePressed es true (ver mas debajo) cuando el enemigo colisiona con una zona en la que puede morir
        if (inputTriggered)
        {
            if(canBePressed)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void onTrigger(int trig)
    {
        _triggersPassed += trig;
    }
    
    public void SetState(int aState)
    {
        _state = aState;

        if (_state == _STATE_ON)
        {
            this.transform.GetComponent<SpriteRenderer>().sprite = _secondPhaseSprite;
        }
    }

    public void SetMovementState(int aState)
    {
        _movement_state = aState;
    }

    public void Killed()
    {
        _activeCoroutine = StartCoroutine(KilledCoroutine());
    }

    public bool currentEye()
    {
        if (mCurrentEye == 1)
        {
            return _leftEye;
        }
        else if (mCurrentEye == 2)
        {
            return _middleEye;
        }
        else
        {
            return _rightEye;
        }
    }

    public bool popEye()
    {
        if (mCurrentEye == 1)
        {
            // Pop first eye
        }
        else if (mCurrentEye == 2)
        {
            // Pop second eye
        }
        else if (mCurrentEye == 3)
        {
            // Pop last eye.

            return true;
        }

        mCurrentEye += 1;

        return false;
    }

    public bool ReturnRightStick()
    {
        return _rightStick;
    }

    private IEnumerator DestroyBySelfCoroutine()
    {
        Destroy(this.gameObject);
        CEnemyManager.Inst.ImOut(this);
        CScoreManager.Inst.BrokeCombo();

        //Do something bad

        yield return null;
    }

    private IEnumerator KilledCoroutine()
    {
        Destroy(this.gameObject);
        CEnemyManager.Inst.ImOut(this);
        CScoreManager.Inst.AddToScore();

        //Do something

        yield return null;
    }

    private void CheckOffStair()
    {
        
        if (transform.position.y <= _lineY)
            SetMovementState(_STATE_OFF_STAIRS);

    }

    public void getHit(int aDamage)
    {
        currentHealth -= aDamage;

        Debug.Log("hit! health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            Killed();
        }
    }

}
