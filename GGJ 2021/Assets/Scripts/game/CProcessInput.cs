using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CProcessInput
{
    public void processInput(int aType, object aValues)
    {

        CEnemy enemy = CEnemyManager.Inst.FirstEnemy();

        if (enemy == null || enemy._state == CEnemy._STATE_OFF)
        {
            Debug.Log("enemy is null!");
            return;
        }

        switch (aType)
        {
            case CSingingStage.DEVICE_MOUSE:

                if (enemy._line != 2)
                {
                    return;
                }

                int value = (int)aValues;

                int pitch = enemy._pitch;

                Debug.Log("enemy pitch: " + pitch + "value: " + value);

                if (pitch == value)
                {
                    enemy.Killed();
                }

                // Audio variance.
                //if (value == CSingingStage.PITCH_LOW)
                //{

                //}
                //else if (value == CSingingStage.PITCH_MID)
                //{

                //}
                //else if (value == CSingingStage.PITCH_HIGH)
                //{

                //}

                    break;
            case CSingingStage.DEVICE_KEYBOARD:

                if (enemy._line != 1)
                {
                    return;
                }
                bool leftInput = (bool)aValues;

                if (leftInput)
                {

                }
                else
                {

                }
                break;

            case CSingingStage.DEVICE_JOYSTICK:
                if (enemy._line != 3)
                {
                    return;
                }

                break;

        }
    }
}
