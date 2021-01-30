using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemy : MonoBehaviour, ITriggered
{
    private static int _STATE_OFF = 0;
    private static int _STATE_ON = 1;

    public int _triggersPassed = 0;

    public int line;

    public bool canBePressed;

    public bool inputTriggered;

    public int _state;

    private Coroutine _activeCoroutine;
    
    private void Start() 
    {
        SetState(_STATE_OFF);
    }


    // Update is called once per frame
    void Update()
    {
        if (_triggersPassed == 1)
        {
            SetState(_STATE_ON);
        }

        Debug.Log(_state);
        Debug.Log(_triggersPassed);

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
    }

    private IEnumerator DestroyBySelfCoroutine()
    {
        Destroy(this.gameObject);
        CEnemyManager.Inst.ImOut(this);
        CScoreManager.Inst.BrokeCombo();

        //Do something bad

        yield return null;
    }
}
