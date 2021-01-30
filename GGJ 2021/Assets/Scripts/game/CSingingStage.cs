using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Voice pitches
    public const int PITCH_LOW = 0;
    public const int PITCH_MID = 1;
    public const int PITCH_HIGH = 2;

    // Current highlighted device.
    private int mCurrentDevice = DEVICE_MOUSE;

    // Min distance to detect mouse movement.
    public float _mouseMinDistance = 30;

    public float _distanceFromCenterToMouseBoundaries = 0.35f;

    private float mTopLine;
    private float mBotLine;


    private int mCurrentPitch = PITCH_MID;

    public GameObject _linePrefab;

    private Image mLineOne;
    private Image mLineTwo;


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
        // Mouse
        mouseLogic();

        // Keyboard
        keyboardLogic();

        // Joystick
        joystickLogic();
    }

    private void mouseLogic()
    {
        // Get the current mouse position.
        mCurrentMousePos = Input.mousePosition.y;

        if (Mathf.Abs(mCurrentMousePos - mLastMousePos) >= _mouseMinDistance)
        {
            //Highlight if not highlighted.
            updateHighlight(DEVICE_MOUSE);
        }

        if (mCurrentMousePos >= mTopLine && mCurrentPitch != PITCH_HIGH)
        {
            mCurrentPitch = PITCH_HIGH;

            Debug.Log("high now!");
        }
        else if (mCurrentMousePos < mTopLine && mCurrentMousePos >= mBotLine && mCurrentPitch != PITCH_MID)
        {
            mCurrentPitch = PITCH_MID;

            Debug.Log("mid now!");

        }
        else if (mCurrentMousePos < mBotLine && mCurrentPitch != PITCH_LOW)
        {
            mCurrentPitch = PITCH_LOW;

            Debug.Log("low now!");

        }
    }

    private void keyboardLogic()
    {
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
    }

    private void joystickLogic()
    {
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
            //else if (Input.GetKeyDown(KeyCode.D))
            //{
            //    mPlayerOne.StopVibration();
            //}
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
        float dist = Camera.main.pixelHeight * _distanceFromCenterToMouseBoundaries;

        Debug.Log("dist: " + dist);

        mBotLine = Camera.main.pixelHeight * 0.5f - dist;
        mTopLine = Camera.main.pixelHeight * 0.5f + dist;

        //Vector3 pos1 = Camera.main.ScreenToWorldPoint(new Vector2(x, mTopLine));
        //Vector3 pos2 = Camera.main.ScreenToWorldPoint(new Vector2(x, mBotLine));

        mLineOne = Instantiate(_linePrefab, _canvas.transform).GetComponent<Image>();
        mLineTwo = Instantiate(_linePrefab, _canvas.transform).GetComponent<Image>();

        mLineOne.transform.localPosition = new Vector2(0, mBotLine - Camera.main.pixelHeight * 0.5f);
        mLineTwo.transform.localPosition = new Vector2(0,  mTopLine - Camera.main.pixelHeight * 0.5f);


        //mLineTwo = Instantiate(_linePrefab, );
    }

    public override void exitState()
    {
        base.exitState();
    }

}
