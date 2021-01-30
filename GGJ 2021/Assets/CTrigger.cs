using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        Object obj = col.gameObject.GetComponent(typeof(ITriggered));
        if (obj != null)
        {
            (obj as ITriggered).onTrigger(1);
        }
    }
}
