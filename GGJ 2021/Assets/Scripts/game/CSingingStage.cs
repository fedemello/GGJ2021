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

    public float _distanceFromCenterToMouseBoundaries = 0.35f;

    private float mTopLine;
    private float mBotLine;

    public GameObject _linePrefab;

    private GameObject mLineOne;
    private GameObject mLineTwo;


    // Drums.
    private bool mPressedLeftDrum = true;

    // Last mouse position in the Y axis.
    private float mCurrentMousePos = 0;
    private float mLastMousePos = 0;

    public Canvas _canvas;

    public override void init()
    {
        base.init();

        createLines();

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

        if (Mathf.Abs(mCurrentMousePos - mLastMousePos) >= _mouseMinDistance)
        {
            //Highlight if not highlighted.
            updateHighlight(DEVICE_MOUSE);
        }

        //Debug.Log("last pos: " + mLastMousePos + " currentPos: " + mCurrentMousePos);

        if (Input.GetKeyDown(KeyCode.N))
        {
            //Left button
            updateHighlight(DEVICE_KEYBOARD);

            mPressedLeftDrum = true;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            //Right button
            updateHighlight(DEVICE_KEYBOARD);

            mPressedLeftDrum = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause?
        }



        // Joystick
        if (mPlayerOne != null)
        {
            // Get current analog direction. Do stuff.
            //mPlayerOne.Direction.;

            if (mPlayerOne.AnyButtonIsPressed)
            {
                // Highlight if not highlighted.
                updateHighlight(DEVICE_JOYSTICK);
            }

            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    mPlayerOne.Vibrate(1);
            //}
            //else if (Input.GetKeyDown(KeyCode.A))
            //{
            //    mPlayerOne.Vibrate(0, 5);
            //}
            //else if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    mPlayerOne.Vibrate(5, 0);
            //}
            //else if (Input.GetKeyDown(KeyCode.W))
            //{
            //    mPlayerOne.Vibrate(5, 0);
            //}
            //else if (Input.GetKeyDown(KeyCode.D))
            //{
            //    mPlayerOne.StopVibration();
            //}
        }
    }

    private void checkKeyboardInput()
    {
        // Keyboard (avoid mouse input)
        if (!Input.GetKey(KeyCode.Mouse0)
              && !Input.GetKey(KeyCode.Mouse1)
              && !Input.GetKey(KeyCode.Mouse2)
              && !Input.GetKey(KeyCode.Mouse3)
              && !Input.GetKey(KeyCode.Mouse4)
              && !Input.GetKey(KeyCode.Mouse5)
              && !Input.GetKey(KeyCode.Mouse6))
        {
            if (Input.anyKeyDown)
            {
                updateHighlight(DEVICE_KEYBOARD);
            }
        }
    }

    private void updateHighlight(int aType)
    {
        if (mCurrentDevice != aType)
        {
            int previousHighlight = mCurrentDevice;
            // Unhighlight previous spot.

            // Save new device.
            mCurrentDevice = aType;

            //Highlight new device.
            Debug.Log("new device: " + mCurrentDevice);


            if (previousHighlight == DEVICE_MOUSE)
            {
                // Update last position for detecting.
                mLastMousePos = mCurrentMousePos;
            }
        }
    }

    public void createLines()
    {
        float x = Camera.main.pixelWidth / 2.0f;

        float middle = Camera.main.pixelHeight / 2.0f;
        float dist = middle * (_distanceFromCenterToMouseBoundaries / 2.0f);

        Debug.Log("pixelMiddle: " + middle + " dist: " + dist);

        mTopLine = middle - dist;
        mBotLine = middle + dist;

        mLineOne = Instantiate(_linePrefab, new Vector2(x, mTopLine), Quaternion.identity);
        mLineTwo = Instantiate(_linePrefab, new Vector2(x, mBotLine), Quaternion.identity);

        //mLineTwo = Instantiate(_linePrefab, );
    }

    public override void exitState()
    {
        base.exitState();
    }

}
