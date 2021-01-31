using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSingingStage : CStateBase
{

    //Music references
    public CSinger _singer;
    public CSinger _drumer;
    public CSinger _guitarrist;

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

    // Joystick directions
    public const int DIR_NONE = -1;
    public const int DIR_NORTH = 0;
    public const int DIR_WEST = 1;
    public const int DIR_SOUTH = 2;
    public const int DIR_EAST = 3;

    private int mCurrentDir = DIR_NONE;
    private int mPreviousDir = DIR_NONE;

    // Current highlighted device.
    private int mCurrentDevice = -1;

    // Min distance to detect mouse movement.
    public float _mouseMinDistance = 30;

    public float _distanceFromCenterToMouseBoundaries = 0.35f;

    private float mTopLine;
    private float mBotLine;


    private int mCurrentPitch = PITCH_MID;

    public GameObject _linePrefab;

    private Image mLineOne;
    private Image mLineTwo;

    private CProcessInput _inputProcessing;



    // Drums.
    private bool mPressedLeftDrum = true;

    // Last mouse position in the Y axis.
    private float mCurrentMousePos = 0;
    private float mLastMousePos = 0;

    public Canvas _canvas;

    public float _joystickMarginValue = 0.20f;
    public float _joystickResetMarginValue = 0.07f;


    public override void init()
    {
        base.init();

        _inputProcessing = new CProcessInput();

        createLines();

        setState(STATE_INTRO);
    }

    public override void setState(int aState)
    {
        base.setState(aState);

        if (aState == STATE_INTRO)
        {
            updateHighlight(DEVICE_MOUSE);
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

        if (mCurrentDevice == DEVICE_MOUSE)
        {
            _inputProcessing.processInput(mCurrentDevice, mCurrentPitch);
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

        if (mCurrentDevice == DEVICE_KEYBOARD)
        {
            _inputProcessing.processInput(mCurrentDevice, mPressedLeftDrum);
        }
    }

    private void joystickLogic()
    {
        if (mPlayerOne != null)
        {
            if (mPlayerOne.AnyButtonIsPressed)
            {
                // Highlight if not highlighted.
                updateHighlight(DEVICE_JOYSTICK);
            }

            // Get current analog direction. Do stuff.
            float currentX = mPlayerOne.Direction.X;
            float currentY = mPlayerOne.Direction.Y;

            bool didGood = true;

            int previousCurrentDir = mCurrentDir;

            // Close to Top
            if (Mathf.Sqrt(Mathf.Pow(currentX, 2) + Mathf.Pow((1 - currentY), 2)) <= _joystickMarginValue)
            {
                Debug.Log("TOP!");

                didGood = setDir(DIR_NORTH);
            }
            // Bot
            else if(Mathf.Sqrt(Mathf.Pow(currentX, 2) + Mathf.Pow((1 + currentY), 2)) <= _joystickMarginValue)
            {
                Debug.Log("BOT!");
                didGood = setDir(DIR_SOUTH);

            }
            // Right
            else if (Mathf.Sqrt(Mathf.Pow(currentY, 2) + Mathf.Pow((1 - currentX), 2)) <= _joystickMarginValue)
            {
                Debug.Log("RIGHT!");
                didGood = setDir(DIR_EAST);

            }
            // Left
            else if (Mathf.Sqrt(Mathf.Pow(currentY, 2) + Mathf.Pow((1 + currentX), 2)) <= _joystickMarginValue)
            {
                Debug.Log("LEFT!");
                didGood = setDir(DIR_WEST);

            }
            // Reset.
            else if (mCurrentDir != DIR_NONE && 
                Mathf.Sqrt(Mathf.Pow(currentX, 2) + Mathf.Pow(currentY, 2)) <= _joystickResetMarginValue)
            {
                Debug.Log("MIDDLE!");

                // stop any vibration.
                mPlayerOne.StopVibration();

                mCurrentDir = DIR_NONE;
            }

            if (previousCurrentDir != mCurrentDir)
            {
                if (didGood)
                {
                    // stop any vibration.
                    mPlayerOne.StopVibration();
                }
                else
                {
                    // wrong!
                    Debug.Log("Vibrate!");
                    mPlayerOne.Vibrate(0.70f);
                }
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

        //if (mCurrentDevice == DEVICE_JOYSTICK)
        //{
        //    _inputProcessing.processInput(mCurrentDevice, mPressedLeftDrum);
        //}
    }

    private bool setDir(int aDir)
    {
        if (mCurrentDir == DIR_NONE)
        {
            mCurrentDir = aDir;
        }
        else
        {
            switch (aDir)
            {
                case DIR_NORTH:
                    if (mCurrentDir == DIR_SOUTH)
                    {
                        return true;
                    }
                    break;
                case DIR_WEST:
                    if (mCurrentDir == DIR_EAST)
                    {
                        return true;
                    }
                    break;
                case DIR_SOUTH:
                    if (mCurrentDir == DIR_NORTH)
                    {
                        return true;
                    }
                    break;
                case DIR_EAST:
                    if (mCurrentDir == DIR_WEST)
                    {
                        return true;
                    }
                    break;
            }

            // We have advanced.
            mPreviousDir = mCurrentDir;

            mCurrentDir = aDir;

            // Send to proces.
            if (mCurrentDevice == DEVICE_JOYSTICK)
            {
                return _inputProcessing.processInput(mCurrentDevice, new Vector2(mPreviousDir, mCurrentDir));
            }
        }

        return true;
    }

    private void updateHighlight(int aType)
    {
        if (mCurrentDevice != aType)
        {
            int previousHighlight = mCurrentDevice;
            // Unhighlight previous spot.

            if (previousHighlight == DEVICE_MOUSE)
            {
                _singer.Off();
                CLineManager.Inst.MidLineOff();
            }
            else if (previousHighlight == DEVICE_KEYBOARD)
            {
                _drumer.Off();
                CLineManager.Inst.TopLineOff();
            }
            else if (previousHighlight == DEVICE_JOYSTICK)
            {
                _guitarrist.Off();
                CLineManager.Inst.BottomLineOff();
            }

            // Save new device.
            mCurrentDevice = aType;


            if (mCurrentDevice == DEVICE_MOUSE)
            {
                _singer.On();
                CLineManager.Inst.MidLineOn();
            }
            else if (mCurrentDevice == DEVICE_KEYBOARD)
            {
                _drumer.On();
                CLineManager.Inst.TopLineOn();
            }
            else if (mCurrentDevice == DEVICE_JOYSTICK)
            {
                _guitarrist.On();
                CLineManager.Inst.BottomLineOn();
            }

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

        //mLineOne = Instantiate(_linePrefab, _canvas.transform).GetComponent<Image>();
        //mLineTwo = Instantiate(_linePrefab, _canvas.transform).GetComponent<Image>();

        //mLineOne.transform.localPosition = new Vector2(0, mBotLine - Camera.main.pixelHeight * 0.5f);
        //mLineTwo.transform.localPosition = new Vector2(0,  mTopLine - Camera.main.pixelHeight * 0.5f);


        //mLineTwo = Instantiate(_linePrefab, );
    }

    public override void exitState()
    {
        base.exitState();
    }

}
