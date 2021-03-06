﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemy : MonoBehaviour, ITriggered
{
    private Animator _anim;

    public static int _STATE_OFF = 0;
    public static int _STATE_ON = 1;

    public static int _STATE_ON_STAIRS = 0;
    public static int _STATE_OFF_STAIRS = 1;
    public static int _STATE_DEATH = 2;


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

    private List<int> _triggersActivated = new List<int>();

    public SpriteRenderer _leftEyeSpr;
    public SpriteRenderer _middleEyeSpr;
    public SpriteRenderer _rightEyeSpr;

    public Color _leftCol;
    public Color _rightCol;

    public AudioClip _explosion;
    public AudioClip _swift;

    private float _deathVolume = 0.2f;

    private void Awake() 
    {
        _anim = GetComponent<Animator>();

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
        else if (_movement_state == _STATE_DEATH)
        {
            return;
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

    public void onTrigger(int trig, int id)
    {
        if (!_triggersActivated.Contains(id))
        {
            _triggersPassed += trig;

            _triggersActivated.Add(id);
        }
    }

    public void EyesOff()
    {
        _leftEyeSpr.enabled = false;
        _middleEyeSpr.enabled = false;
        _rightEyeSpr.enabled = false;
    }
    
    public void SetState(int aState)
    {
        _state = aState;

        if (_state == _STATE_ON)
        {
            _anim.SetTrigger("Trigger1");

            _leftEyeSpr.gameObject.SetActive(true);
            _middleEyeSpr.gameObject.SetActive(true);
            _rightEyeSpr.gameObject.SetActive(true);

            if (_line == 1)
            {
                _leftEyeSpr.color = _leftEye ? _leftCol : _rightCol;
                _middleEyeSpr.color = _middleEye ? _leftCol : _rightCol;
                _rightEyeSpr.color = _rightEye ? _leftCol : _rightCol;

            }
            else if (_line == 2)
            {
                if (CEnemyManager.Inst._firstEnemy == this)
                {
                    CEnemyManager.Inst.playEnemySfx();
                }
            }

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
        // Get getPushed.
        getPushed(CSingingStage.PUSH_DRUM);

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
        
        // Increment current eye.
        mCurrentEye += 1;

        return false;
    }

    public bool ReturnRightStick()
    {
        return _rightStick;
    }

    private IEnumerator DestroyBySelfCoroutine()
    {
        if (_line == 2)
        {
            CEnemyManager.Inst.stopEnemySfx();
        }

        CAudioManager.Inst.playSfx("self_destruct", _swift, _deathVolume);

        SetMovementState(_STATE_DEATH);
        _anim.SetTrigger("Trigger3");
        CEnemyManager.Inst.ImOut(this);
        EyesOff();

        yield return new WaitForSeconds(1.3f);

        Destroy(this.gameObject);

        //Do something bad
        CScoreManager.Inst.updateBarAndClassification();

        yield return null;
    }

    private IEnumerator KilledCoroutine()
    {
        if (_line == 2)
        {
            CEnemyManager.Inst.stopEnemySfx();
        }

        CAudioManager.Inst.playSfx("explosion", _explosion, _deathVolume);


        SetMovementState(_STATE_DEATH);
        _anim.SetTrigger("Trigger2");
        CEnemyManager.Inst.ImOut(this);
        EyesOff();

        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);

        CSingingStage SS = CLevelManager.Inst.getCurrentState() as CSingingStage;

        if ( SS != null)
        {
            if (!(SS.tutorialEnabled))
            {
                CScoreManager.Inst.AddToScore(50);
            }
        }



        

        

        //Do something

        yield return null;
    }

    private void CheckOffStair()
    {
        
        if (transform.position.y <= _lineY)
            SetMovementState(_STATE_OFF_STAIRS);

    }

    public void getHit(int aDamage, int pushAmount = 0)
    {
        if(currentHealth <= 0)
            return;

        currentHealth -= aDamage;

        Debug.Log("hit! health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            Killed();
        }
        else
        {
            getPushed(pushAmount);
        }
    }

    public void getPushed(int amount)
    {
        transform.position += new Vector3(amount, 0f, 0f);
    }

}
