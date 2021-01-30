using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSingingStage : CStateBase
{
    // States.
    public const int STATE_INTRO = 0;
    public const int STATE_PLAYING = 1;
    public const int STATE_ENDING = 2;

    // Devices
    public const int DEVICE_MOUSE = 0;
    public const int DEVICE_KEYBOARD = 1;
    public const int DEVICE_JOYSTICK = 2;

    // Current highlighted device.
    private int mCurrentDevice = DEVICE_MOUSE;

    // Min distance to detect mouse movement.
    public float _mouseMinDistance = 30;

    // Last mouse position in the Y axis.
    private float mCurrentMousePos = 0;
    private float mLastMousePos = 0;

    public override void init()
    {
        base.init();

        setState(STATE_INTRO);
    }

    public override void setState(int aState)
    {
        base.setState(aState);

        if (aState == STATE_INTRO)
        {

        }
        else if (aState == STATE_PLAYING)
        {

        }
        else if (aState == STATE_ENDING)
        {

        }
    }

    public override void update()
    {
        base.update();

        if (mState == STATE_INTRO)
        {
            // No intro for now.
            setState(STATE_PLAYING);
        }
        else if (mState == STATE_PLAYING)
        {
            // Check controllers
            checkControllerInput();
        }
        else if (mState == STATE_ENDING)
        {

        }
    }

    public void checkControllerInput()
    {
        // MOUSE
        // Get the current mouse position.
        mCurrentMousePos = Input.mousePosition.y;

        Debug.Log("last pos: " + mLastMousePos + " currentPos: " + mCurrentMousePos);

        // At the end, update last position with current.
        mLastMousePos = mCurrentMousePos;

        // Keyboard
        if (Input.GetKeyDown(KeyCode.X))
        {

        }

        // Joystick

        if (mPlayerOne != null)
        {
            // Get current analog direction. Do stuff.
            //mPlayerOne.Direction.;
        }        
    }

    public override void exitState()
    {
        base.exitState();
    }

}
