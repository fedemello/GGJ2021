using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CProcessInput
{

    public bool processInput(int aType, object aValues, int aDamage = 0, int aPush = 0)
    {


        CEnemy enemy = CEnemyManager.Inst.FirstEnemy();

        if (enemy == null)
        {
            Debug.Log("enemy is null!");
            return true;
        }

        if (enemy._state == CEnemy._STATE_OFF)
        {
            return true;
        }

        switch (aType)
        {
            case CSingingStage.DEVICE_MOUSE:

                if (enemy._line != 2)
                {
                    return true;
                }

                int value = (int)aValues;

                int pitch = enemy._pitch;

                //Debug.Log("enemy pitch: " + pitch + "value: " + value);

                if (pitch == value)
                {
                    enemy.getHit(aDamage, aPush);
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
                    return true;
                }
                bool leftInput = (bool)aValues;

                bool enemyInput = enemy.currentEye();

                if (leftInput == enemyInput)
                {
                    // If last eye.
                    if (enemy.popEye())
                    {
                        enemy.Killed();
                    }
                }

                break;

            case CSingingStage.DEVICE_JOYSTICK:
                if (enemy._line != 3)
                {
                    return true;
                }

                Vector2 directions = (Vector2)aValues;

                int previous = (int)directions.x;
                int current = (int)directions.y;

                switch(previous)
                {
                    case CSingingStage.DIR_NORTH:
                        if (enemy.ReturnRightStick())
                        {
                            if (current == CSingingStage.DIR_WEST)
                            {
                                //Hit!
                                enemy.getHit(aDamage, aPush);
                            }
                            else if (current == CSingingStage.DIR_EAST)
                            {
                                // Wrong!.
                                return false;
                            }
                        }
                        else
                        {
                            if (current == CSingingStage.DIR_EAST)
                            {
                                // Hit!
                                enemy.getHit(aDamage, aPush);

                            }
                            else if (current == CSingingStage.DIR_WEST)
                            {
                                // Wrong!
                                return false;
                            }
                        }
                        break;
                    case CSingingStage.DIR_WEST:
                        if (enemy.ReturnRightStick())
                        {
                            if (current == CSingingStage.DIR_SOUTH)
                            {
                                //Hit!
                                enemy.getHit(aDamage, aPush);

                            }
                            else if (current == CSingingStage.DIR_NORTH)
                            {
                                // Wrong!.
                                return false;
                            }
                        }
                        else
                        {
                            if (current == CSingingStage.DIR_NORTH)
                            {
                                // Hit!
                                enemy.getHit(aDamage, aPush);

                            }
                            else if (current == CSingingStage.DIR_SOUTH)
                            {
                                // Wrong!
                                return false;
                            }
                        }
                        break;
                    case CSingingStage.DIR_SOUTH:
                        if (enemy.ReturnRightStick())
                        {
                            if (current == CSingingStage.DIR_EAST)
                            {
                                //Hit!
                                enemy.getHit(aDamage, aPush);

                            }
                            else if (current == CSingingStage.DIR_WEST)
                            {
                                // Wrong!.
                                return false;
                            }
                        }
                        else
                        {
                            if (current == CSingingStage.DIR_WEST)
                            {
                                // Hit!
                                enemy.getHit(aDamage, aPush);

                            }
                            else if (current == CSingingStage.DIR_EAST)
                            {
                                // Wrong!
                                return false;
                            }
                        }
                        break;
                    case CSingingStage.DIR_EAST:
                        if (enemy.ReturnRightStick())
                        {
                            if (current == CSingingStage.DIR_NORTH)
                            {
                                //Hit!
                                enemy.getHit(aDamage, aPush);

                            }
                            else if (current == CSingingStage.DIR_SOUTH)
                            {
                                // Wrong!.
                                return false;
                            }
                        }
                        else
                        {
                            if (current == CSingingStage.DIR_SOUTH)
                            {
                                // Hit!
                                enemy.getHit(aDamage, aPush);

                            }
                            else if (current == CSingingStage.DIR_NORTH)
                            {
                                // Wrong!
                                return false;
                            }
                        }
                        break;
                }
                break;
        }

        return true;
    }
}
