using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COutro : MonoBehaviour
{
    private Animator _anim;

    public bool _creditosEnded = false;

    private Coroutine _activeCoroutine;

    private void Awake() 
    {
        _anim = GetComponent<Animator>();    
    }

    public void Credits()
    {
        _anim.SetTrigger("Credits");

        _activeCoroutine = StartCoroutine(WaiterCoroutine());
    }

    private IEnumerator WaiterCoroutine()
    {
        yield return new WaitForSeconds(2f);

        _creditosEnded = true;

        yield return null;
    }
}
