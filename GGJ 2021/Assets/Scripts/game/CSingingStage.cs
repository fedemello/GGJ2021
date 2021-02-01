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
    public Transform _camera;
    public Vector3 _cameraInicialPos;
    public CIntro _intro;
    public bool _endedIntro = false;

    // States.
    public const int STATE_INTRO = 0;
    public const int STATE_MENU = 3;
    public const int STATE_PLAYING = 1;
    public const int STATE_ENDING = 2;

    public bool tutorialEnabled = false;

    private int mCurrentTutorialStage = 1;

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

    // Device damage amounts.
    public int DAMAGE_SING = 4;
    public int DAMAGE_GUITAR = 25;


    // Device push amounts.
    public const int PUSH_SING = 1;
    public const int PUSH_DRUM = 10;
    public const int PUSH_GUITAR = 5;

    private int mCurrentDir = DIR_NONE;
    private int mPreviousDir = DIR_NONE;

    // Current highlighted device.
    private int mCurrentDevice = -1;

    // Min distance to detect mouse movement.
    public float _mouseMinDistance = 30;

    public float _distanceFromCenterToMouseBoundaries = 0.35f;

    private float mTopLine;
    private float mBotLine;

    public CEnemyScroller _enemyScroller;

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

    public AudioClip _singing;
    public AudioClip _drums;
    public AudioClip _guitar;

    public float _standardVolume = 0.2f;
    public float _highlightedVolume = 0.8f;


    public List<AudioClip> _drumSfx = new List<AudioClip>();
    public AudioClip _guitarIntro;

    private int mCurrentDrumSfx = 0;

    public AudioClip _singingMusic;

    public float _endingTime;


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
            _camera.position = _cameraInicialPos;
            _intro.Intro();

            StartCoroutine(IntroCoroutine());
            
            updateHighlight(DEVICE_MOUSE);
        }
        else if (aState == STATE_PLAYING)
        {
            CAudioManager.Inst.startMusic(_singingMusic);

            CAudioManager.Inst.playSfx("sing", _singing, _standardVolume);
            CAudioManager.Inst.playSfx("drum", _drums, _standardVolume);
            CAudioManager.Inst.playSfx("guitar", _guitar, _standardVolume);

            _enemyScroller.Spawn();
        }
        else if (aState == STATE_ENDING)
        {
            Debug.Log("END!");

            if (mPlayerOne != null)
            {
                mPlayerOne.StopVibration();
            }
        }
        else if (aState == STATE_MENU)
        {

        }

    }

    public IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(6.5f);

        _endedIntro = true;

        yield return null;
    }

    public override void update()
    {
        base.update();

        if (mState == STATE_INTRO)
        {
            // No intro for now.

            if (_endedIntro)
            {
                if (Input.anyKeyDown)
                {
                    setState(STATE_MENU);
                    Destroy(_intro.gameObject);
                }
            }

        }
        else if (mState == STATE_PLAYING)
        {
            // Check controllers
            checkControllerInput();

            if (!CAudioManager.Inst.isMusicPlaying())
            {
                setState(STATE_ENDING);
            }
        }
        else if (mState == STATE_ENDING)
        {
            _endingTime = _endingTime * Time.deltaTime;

            if (_endingTime == 3)
            {
                setState(STATE_MENU);
            }

        }
        else if (mState == STATE_MENU)
        {
            //presionar cualquier tecla

            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    tutorialEnabled = true;
                    setState(STATE_PLAYING);
                }
                else
                {
                    setState(STATE_PLAYING);
                }

            }
            

                
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
            _inputProcessing.processInput(mCurrentDevice, mCurrentPitch, DAMAGE_SING, PUSH_SING);
        }
    }

    private void keyboardLogic()
    {
        bool pressed = false;

        if (Input.GetKeyDown(KeyCode.N))
        {
            updateHighlight(DEVICE_KEYBOARD);

            //Left button
            mPressedLeftDrum = true;
            _drumer.Amarillo();

            pressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            updateHighlight(DEVICE_KEYBOARD);

            //Right button
            mPressedLeftDrum = false;
            _drumer.Azul();

            pressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause?
        }

        if (pressed && mCurrentDevice == DEVICE_KEYBOARD)
        {
            _inputProcessing.processInput(mCurrentDevice, mPressedLeftDrum);

            CAudioManager.Inst.playSfx("drum_" + mCurrentDrumSfx, _drumSfx[mCurrentDevice]);

            mCurrentDrumSfx += 1;

            if (mCurrentDrumSfx >= _drumSfx.Count)
            {
                mCurrentDrumSfx = 0;
            }
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

                _guitarrist.GuitarOff();

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
        }
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

            // Highlight if not highlighted.
            updateHighlight(DEVICE_JOYSTICK);

            //Anim guitar -------------------------------------------------

            _guitarrist.GuitarOn();

            // Send to proces.
            if (mCurrentDevice == DEVICE_JOYSTICK)
            {
                return _inputProcessing.processInput(mCurrentDevice, new Vector2(mPreviousDir, mCurrentDir), DAMAGE_GUITAR, PUSH_GUITAR);
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

                CAudioManager.Inst.setSfxVolume("sing", _standardVolume);
            }
            else if (previousHighlight == DEVICE_KEYBOARD)
            {
                _drumer.Off();
                CLineManager.Inst.TopLineOff();

                CAudioManager.Inst.setSfxVolume("drum", _standardVolume);
            }
            else if (previousHighlight == DEVICE_JOYSTICK)
            {
                _guitarrist.Off();
                _guitarrist.GuitarOff();
                CLineManager.Inst.BottomLineOff();

                CAudioManager.Inst.setSfxVolume("guitar", _standardVolume);

                if (mPlayerOne != null)
                {
                    mPlayerOne.StopVibration();
                }
            }

            // Save new device.
            mCurrentDevice = aType;


            if (mCurrentDevice == DEVICE_MOUSE)
            {
                _singer.On();
                CLineManager.Inst.MidLineOn();

                CAudioManager.Inst.setSfxVolume("sing", _highlightedVolume);

            }
            else if (mCurrentDevice == DEVICE_KEYBOARD)
            {
                _drumer.On();
                CLineManager.Inst.TopLineOn();

                CAudioManager.Inst.setSfxVolume("drum", _highlightedVolume);

            }
            else if (mCurrentDevice == DEVICE_JOYSTICK)
            {
                _guitarrist.On();
                CLineManager.Inst.BottomLineOn();

                CAudioManager.Inst.setSfxVolume("guitar", _highlightedVolume);

                if (!CAudioManager.Inst.isSfxPlaying("guitar_solo"))
                {
                    CAudioManager.Inst.playSfx("guitar_solo", _guitarIntro);
                }
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

    public int getCurrentTutorialStage()
    {
        return mCurrentTutorialStage;
    }

}
