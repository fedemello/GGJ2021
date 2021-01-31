using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using InControl;
using System;

public class CStateBase : MonoBehaviour
{
    protected int mState;
    protected int mPreviousState; 

    protected float mTimeState = 0;

    [HideInInspector]
    public CGrid mGrid;

    private bool mMenuEnabled = true;

    protected bool mPlayerOneEnabled = true;
    protected bool mPlayerTwoEnabled = true;

    protected InputDevice mPlayerOne;
    protected InputDevice mPlayerTwo;

    [HideInInspector]
    public float STICK_OFFSET = 0.70f;
    [HideInInspector]
    public float TICK_DELAY = 0.3f;
    [HideInInspector]
    public float BACK_STICK_OFFSET = 0.25f;

    private float mPlayerOneDelay = 0;
    private float mPlayerTwoDelay = 0;

    protected int mIndexPlayer1 = 0;
    protected int mIndexPlayer2 = 2;

    private GameObject mPlayerOneUI;
    private GameObject mPlayerTwoUI;

    public const int TYPE_ONE_PLAYER = 0;
    public const int TYPE_TWO_PLAYERS = 1;

    private int mInputType = TYPE_ONE_PLAYER;

    private bool mLoopingMenu = true;

    // Public variables to set at each menu.
    public AudioClip _musica;

    public bool _loopMusic = true;

    [HideInInspector]
    public GameObject _selectorPrefab1;

    [HideInInspector]
    public GameObject _selectorPrefab2;

    [HideInInspector]
    public SpriteRenderer _background;

    [HideInInspector]
    public AudioClip _playerOneMove;

    [HideInInspector]
    public AudioClip _playerTwoMove;

    [HideInInspector]
    public AudioClip _playerOneSelect;

    [HideInInspector]
    public AudioClip _playerTwoSelect;

    [HideInInspector]
    public AudioClip _playerOneDeselect;

    [HideInInspector]
    public AudioClip _playerTwoDeselect;



    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Initialization function. Called from CGame's setState() after the constructor is created and when the state is set.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------------------------
    /// 
    private void Awake()
    {
        if (CLevelManager.Inst == null)
        {
            new CLevelManager();
        }
    }

    virtual public void init()
    {
        //Debug.Log("baseState init()");
        if (CTransitionManager.Inst.IsScreenCovered() == true)
        {
            CTransitionManager.Inst.SetFadeOutFlag();
        }

        if (!CAudioManager.Inst.isMusicPlaying(_musica))
        {
            CAudioManager.Inst.startMusic(_musica, _loopMusic);
        }
            
        mGrid = FindObjectOfType<CGrid>();

        if (mGrid != null)
        {
            mGrid.init();
        }
        else
        {
            Debug.Log("grid null!");
        }
    }

    /// <summary>
    /// set the player controllers after initing the state.
    /// </summary>
    /// <param name="aPlayerOne"></param>
    /// <param name="aPlayerTwo"></param>
    virtual public void setPlayerControllers(InputDevice aPlayerOne, InputDevice aPlayerTwo)
    {
        mPlayerOne = aPlayerOne; 
        mPlayerTwo = aPlayerTwo;

        Debug.Log("state controller set one:" + (mPlayerOne != null).ToString() + " two:" + (mPlayerTwo != null).ToString());
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Sets the current state (FSM state) of the game.
    /// </summary>
    /// <param name="aState">The new state to set.</param>
    /// ------------------------------------------------------------------------------------------------------------------
    virtual public void setState(int aState)
    {
        // Save the previous state.
        mPreviousState = mState;

        //// Call exitState(). Base classes can use this function to clean up things related to the exiting state.
        //exitState(mPreviousState);

        // Save the current state.
        mState = aState;

        // Reset the time in this state.
        mTimeState = 0.0f;
    }

    public int getState()
    {
        return mState;
    }

    /// ------------------------------------------------------------------------------------------------------------------    
    /// <summary>
    /// Update function. Called every frame.
    /// </summary>
    /// ------------------------------------------------------------------------------------------------------------------
    virtual public void update()
    {
        // Get the delta time.
        float aDt = CLevelManager.Inst.getDeltaTime();

        // Add the time in the current state.
        mTimeState += aDt;

        if (mMenuEnabled)
        {
            checkPlayerInput();
        }
    }

    protected void spawnPlayerUI(GameObject obj1, GameObject obj2 = null)
    {
        mPlayerOneUI = Instantiate(obj1);
        mPlayerOneUI.transform.localPosition = Vector3.zero;
        if (obj2 != null)
        {
            mPlayerTwoUI = Instantiate(obj2);
            mPlayerTwoUI.transform.localPosition = Vector3.zero;
        }
    }

    // Set the input type
    public void setInputType(int aType)
    {
        mInputType = aType;
    }
    
    // Enable disable player input (menu movement).
    public void enablePlayerInput(bool aEnable)
    {
        mMenuEnabled = aEnable;
    }

    /// <summary>
    /// Used to destroy objects when changing states.
    /// </summary>
    virtual public void exitState()
    {
        if (mGrid != null)
        {
            mGrid.destroy();
            mGrid = null;
        }
    }

    /// <summary>
    /// Used to load the level menu in the grid.
    /// </summary>
    virtual public void loadMenu()
    {
        // Used to avoid deleting player UI when loadin a menu.
        if (mPlayerOneUI != null)
        {
            mPlayerOneUI.transform.SetParent(mGrid.transform.parent);

            mIndexPlayer1 = 0;
        }
        if (mPlayerTwoUI != null)
        {
            mPlayerTwoUI.transform.SetParent(mGrid.transform.parent);

            mIndexPlayer2 = 2;
        }

        clearGrid();
    }

    public void clearGrid()
    {
        if (mGrid != null)
        {
            mGrid.clear();
        }
    }

    public void setupMenu()
    {
        if (mPlayerOneUI == null)
        {
            spawnPlayerUI(_selectorPrefab1, _selectorPrefab2);
        }

        //RectTransform button = mGrid.getButton(mIndexPlayer1).transform as RectTransform;

        mPlayerOneUI.transform.SetParent(mGrid.getButton(mIndexPlayer1).gameObject.transform);
        mPlayerOneUI.transform.localPosition = Vector3.zero;

        (mPlayerOneUI.transform as RectTransform).sizeDelta = mGrid.getCellSize();

        if (mPlayerTwoUI != null)
        {
            mPlayerTwoUI.transform.SetParent(mGrid.getButton(mIndexPlayer2).gameObject.transform);
            mPlayerTwoUI.transform.localPosition = Vector3.zero;
        }
    }

    public CGrid getGrid()
    {
        return mGrid;
    }

    public CButton createButton(GameObject aPrefab, string aText, Action aOnClick = null, Action aOnUnclick = null)
    {
        CButton button = Instantiate(aPrefab, mGrid.transform).GetComponent<CButton>();
        button.setup(aText, CSpriteLoader.getButtonSprite(), aOnClick, aOnUnclick);
        mGrid.addButton(button);

        return button;
    }


    public void checkPlayerInput()
    {
        
        if (mPlayerOne == null)
        {
            // TODO: Suscribe for next available controller.
            mPlayerOne = CInputManager.Inst.getPlayerControl();
        }

        if (mPlayerOne != null)
        {
            if (mPlayerOneDelay > 0)
            {
                // Player delay gets reduced over time
                mPlayerOneDelay -= Time.deltaTime;

                // If player is back in the middle, we reset to 0.
                if (mPlayerOne.Direction.X > -BACK_STICK_OFFSET && mPlayerOne.Direction.X < BACK_STICK_OFFSET &&
                    mPlayerOne.Direction.Y > -BACK_STICK_OFFSET && mPlayerOne.Direction.Y < BACK_STICK_OFFSET)
                {
                    mPlayerOneDelay = 0;
                }
            }

            if (mPlayerOneDelay <= 0)
            {
                // PLAYER ONE INPUT.
                if (mPlayerOneEnabled)
                {
                    bool moved = false;

                    // Joystick controls.
                    if (mPlayerOne.Direction.X < -STICK_OFFSET)
                    {
                        // Left.
                        if (moveLeft())
                        {
                            moved = true;
                        }
                        
                    }
                    else if (mPlayerOne.Direction.X > STICK_OFFSET)
                    {
                        // Right.
                        if (moveRight())
                        {
                            moved = true;
                        }
                    }
                    else if (mPlayerOne.Direction.Y < -STICK_OFFSET)
                    {
                        // Down.
                        if (moveDown())
                        {
                            moved = true;
                        }
                    }
                    else if (mPlayerOne.Direction.Y > STICK_OFFSET)
                    {
                        // Up.
                        if (moveUp())
                        {
                            moved = true;
                        }
                    }

                    if (moved)
                    {
                        mPlayerOneDelay = TICK_DELAY;
                    }
                }
            }

            if (mPlayerOne.Action1.WasPressed || mPlayerOne.Action3.WasPressed)
            {
                action1Player1();
            }
            else if (mPlayerOne.Action2.WasPressed || mPlayerOne.Action4.WasPressed)
            {
                action2Player1();
            }
            else if (mPlayerOne.CommandWasPressed)
            {
                onButtonBack();
            }
        }
        else
        {
            // PLAYER ONE INPUT.
            if (mPlayerOneEnabled)
            {
                // Keyboard controls.
                if (Input.GetKeyDown(KeyCode.A))
                {
                    // Left.
                    moveLeft();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    // Right.
                    moveRight();
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    // Down.
                    moveDown();
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    // Up.
                    moveUp();
                }
            }

            if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Space))
            {
                action1Player1();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                action2Player1();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                onButtonBack();
            }
        }
        
        if (mPlayerTwo == null)
        {
            mPlayerTwo = CInputManager.Inst.getPlayerControl(false);
        }

        if (mPlayerTwo != null)
        {
            if (mPlayerTwoDelay > 0)
            {
                mPlayerTwoDelay -= Time.deltaTime;

                if (mPlayerTwo.Direction.X > -BACK_STICK_OFFSET && mPlayerTwo.Direction.X < BACK_STICK_OFFSET &&
                    mPlayerTwo.Direction.Y > -BACK_STICK_OFFSET && mPlayerTwo.Direction.Y < BACK_STICK_OFFSET)
                {
                    mPlayerTwoDelay = 0;
                }
            }

            if (mPlayerTwoDelay <= 0)
            {
                // PLAYER TWO INPUT.
                if (mPlayerTwoEnabled)
                {
                    bool moved = false;

                    // Joystick controls.
                    if (mPlayerTwo.Direction.X < -STICK_OFFSET)
                    {
                        // Left.
                        if (moveLeft(false))
                        {
                            moved = true;
                        }
                    }
                    else if (mPlayerTwo.Direction.X > STICK_OFFSET)
                    {
                        // Right.
                        if (moveRight(false))
                        {
                            moved = true;
                        }
                    }
                    else if (mPlayerTwo.Direction.Y < -STICK_OFFSET)
                    {
                        // Down.
                        if (moveDown(false))
                        {
                            moved = true;
                        }
                    }
                    else if (mPlayerTwo.Direction.Y > STICK_OFFSET)
                    {
                        // Up.
                        if (moveUp(false))
                        {
                            moved = true;
                        }
                    }

                    if (moved)
                    {
                        mPlayerTwoDelay = TICK_DELAY;
                    }
                }

            }

            if (mPlayerTwo.Action1.WasPressed || mPlayerTwo.Action3.WasPressed)
            {
                action1Player2();
            }
            else if (mPlayerTwo.Action2.WasPressed || mPlayerTwo.Action4.WasPressed)
            {
                action2Player2();
            }
            else if (mPlayerTwo.CommandWasPressed)
            {
                onButtonBack();
            }
        }
        else
        {
            // PLAYER TWO INPUT.
            if (mPlayerTwoEnabled)
            {
                // Keyboard controls.
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    // Left.
                    moveLeft(false);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    // Right.
                    moveRight(false);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    // Down.
                    moveDown(false);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    // Up.
                    moveUp(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                action1Player2();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                action2Player2();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                onButtonBack();
            }
        }
    }

    private bool moveLeft(bool aPlayerOne = true)
    {
        if (mGrid == null || mGrid.getColumnCount() == 1)
        {
            return false;
        }

        bool foundIndex = false;

        if (aPlayerOne || mInputType == TYPE_ONE_PLAYER)
        {
            while (!foundIndex)
            {
                if (mIndexPlayer1 != 0)
                {
                    mIndexPlayer1 -= 1;
                }
                else
                {
                    mIndexPlayer1 = mGrid.getButtonCount() - 1;
                }

                if (!mGrid.getButton(mIndexPlayer1).isHidden())
                {
                    foundIndex = true;
                }

            }

            updateIndicator();
        }
        else
        {
            while (!foundIndex)
            {
                if (mIndexPlayer2 != 0)
                {
                    mIndexPlayer2 -= 1;
                }
                else
                {
                    mIndexPlayer2 = mGrid.getButtonCount() - 1;
                }

                if (!mGrid.getButton(mIndexPlayer2).isHidden())
                {
                    foundIndex = true;
                }

            }

            updateIndicator(false);
        }

        return true;
    }

    private bool moveRight(bool aPlayerOne = true)
    {

        if (mGrid == null || mGrid.getColumnCount() == 1)
        {
            return false;
        }

        //int endIndex = mButtons.Count - 1;

        //// Shh!!
        //if (!mHiddenPlayerVisible)
        //{
        //    endIndex -= 1;
        //}

        bool foundIndex = false;

        if (aPlayerOne || mInputType == TYPE_ONE_PLAYER)
        {
            

            while (!foundIndex)
            {
                if (mIndexPlayer1 + 1 != mGrid.getButtonCount())
                {
                    mIndexPlayer1 += 1;
                }
                else
                {
                    mIndexPlayer1 = 0;
                }

                if (!mGrid.getButton(mIndexPlayer1).isHidden())
                {
                    foundIndex = true;
                }

            }   

            updateIndicator();
        }
        else
        {
            while (!foundIndex)
            {
                if (mIndexPlayer2 + 1 != mGrid.getButtonCount())
                {
                    mIndexPlayer2 += 1;
                }
                else
                {
                    mIndexPlayer2 = 0;
                }

                if (!mGrid.getButton(mIndexPlayer2).isHidden())
                {
                    foundIndex = true;
                }
            }

            updateIndicator(false);
        }

        return true;
    }

    private bool moveDown(bool aPlayerOne = true)
    {
        if (mGrid == null)
        {
            return false;
        }
        //int endIndex = mButtons.Count - 1;

        //// Shh!!
        //if (!mHiddenPlayerVisible)
        //{
        //    endIndex -= 1;
        //}

        bool foundIndex = false;

        if (aPlayerOne || mInputType == TYPE_ONE_PLAYER)
        {
            int newIndex = mIndexPlayer1;

            while (!foundIndex)
            {
                newIndex += mGrid.getColumnCount();

                Debug.Log("index: " + newIndex);

                if (newIndex < mGrid.getButtonCount())
                {
                    if (!mGrid.getButton(newIndex).isHidden())
                    {
                        foundIndex = true;
                    }
                }
                else
                {
                    if (mLoopingMenu)
                    {
                        newIndex = newIndex - mGrid.getButtonCount();

                        if (!mGrid.getButton(newIndex).isHidden())
                        {
                            foundIndex = true;
                        }
                    }
                    else
                    {
                        while (!foundIndex)
                        {
                            newIndex -= 1;

                            if (newIndex <= mGrid.getButtonCount() && !mGrid.getButton(newIndex).isHidden())
                            {
                                foundIndex = true;
                            }
                        }
                    }
                }
            }

            mIndexPlayer1 = newIndex;

            updateIndicator();
        }
        else
        {
            int newIndex = mIndexPlayer2;

            while (!foundIndex)
            {
                newIndex += mGrid.getColumnCount();

                Debug.Log("index: " + newIndex);

                if (newIndex < mGrid.getButtonCount())
                {
                    if (!mGrid.getButton(newIndex).isHidden())
                    {
                        foundIndex = true;
                    }
                }
                else
                {
                    if (mLoopingMenu)
                    {
                        newIndex = newIndex - mGrid.getButtonCount();

                        if (!mGrid.getButton(newIndex).isHidden())
                        {
                            foundIndex = true;
                        }
                    }
                    else
                    {
                        while (!foundIndex)
                        {
                            newIndex -= 1;

                            if (newIndex <= mGrid.getButtonCount() && !mGrid.getButton(newIndex).isHidden())
                            {
                                foundIndex = true;
                            }
                        }
                    }
                }
            }

            mIndexPlayer2 = newIndex;

            updateIndicator(false);
        }

        return true;
    }

    private bool moveUp(bool aPlayerOne = true)
    {
        if (mGrid == null)
        {
            return false;
        }

        int endIndex = mGrid.getButtonCount() - 1;
        int newIndex;

        bool foundIndex = false;

        if (aPlayerOne || mInputType == TYPE_ONE_PLAYER)
        {
            newIndex = mIndexPlayer1;

            while (!foundIndex)
            {
                newIndex -= mGrid.getColumnCount();

                if (newIndex < 0)
                {
                    if (mLoopingMenu)
                    {
                        newIndex += endIndex + 1;

                        if (newIndex > endIndex)
                        {
                            newIndex = endIndex;
                        }
                    }
                    else
                    {
                        newIndex = -1;

                        while(!foundIndex)
                        {
                            newIndex += 1;

                            if (!mGrid.getButton(newIndex).isHidden())
                            {
                                foundIndex = true;
                            }
                        }
                    }
                }

                if (!mGrid.getButton(newIndex).isHidden())
                {
                    foundIndex = true;
                }
            }

            mIndexPlayer1 = newIndex;

            updateIndicator();
        }
        else
        {
            newIndex = mIndexPlayer2;

            while (!foundIndex)
            {
                newIndex -= mGrid.getColumnCount();

                if (newIndex < 0)
                {
                    if (mLoopingMenu)
                    {
                        newIndex += endIndex + 1;

                        if (newIndex > endIndex)
                        {
                            newIndex = endIndex;
                        }
                    }
                    else
                    {
                        newIndex = -1;

                        while (!foundIndex)
                        {
                            newIndex += 1;

                            if (!mGrid.getButton(newIndex).isHidden())
                            {
                                foundIndex = true;
                            }
                        }
                    }
                }

                if (!mGrid.getButton(newIndex).isHidden())
                {
                    foundIndex = true;
                }
            }

            mIndexPlayer2 = newIndex;

            updateIndicator(false);
        }

        return true;
    }

    public virtual void updateIndicator(bool aPlayerOne = true)
    {
        if (aPlayerOne)
        {
            Debug.Log("playerOne moved, new Index: " + mIndexPlayer1);
            mPlayerOneUI.transform.SetParent(mGrid.getButton(mIndexPlayer1).gameObject.transform);
            mPlayerOneUI.transform.localPosition = Vector2.zero;

            if (_playerOneMove != null)
            {
                CAudioManager.Inst.playSfx("move1", _playerOneMove);
            }
            
        }
        else
        {
            Debug.Log("playerTwo moved, new Index: " + mIndexPlayer2);
            mPlayerTwoUI.transform.SetParent(mGrid.getButton(mIndexPlayer2).gameObject.transform);
            mPlayerTwoUI.transform.localPosition = Vector2.zero;

            if (_playerTwoMove != null)
            {
                CAudioManager.Inst.playSfx("move2", _playerTwoMove);
            }
            else
            {
                if (_playerOneMove != null)
                {
                    CAudioManager.Inst.playSfx("move1", _playerOneMove);
                }
            }
        }
    }

    public virtual void action1Player1()
    {
        if (mGrid != null)
        {
            if (mIndexPlayer1 >= 0 && mIndexPlayer1 <= mGrid.getButtonCount())
            {
                //Debug.Log("p1 action index: " + mIndexPlayer1);

                mGrid.getButton(mIndexPlayer1).onButtonClicked();
            }
        }

        if (_playerOneSelect != null)
        {
            CAudioManager.Inst.playSfx("confirm1", _playerOneSelect);
        }
    }
    public virtual void action2Player1()
    {
        if (mGrid != null)
        {
            if (mIndexPlayer1 >= 0 && mIndexPlayer1 <= mGrid.getButtonCount())
            {
                mGrid.getButton(mIndexPlayer1).onButtonUnclicked();
            }
        }

        if (_playerOneDeselect != null)
        {
            CAudioManager.Inst.playSfx("unconfirm1", _playerOneDeselect);
        }
    }
    public virtual void action1Player2()
    {
        if (mGrid != null)
        {
            if (mInputType == TYPE_TWO_PLAYERS)
            { 
                if (mIndexPlayer2 >= 0 && mIndexPlayer2 <= mGrid.getButtonCount())
                {
                    mGrid.getButton(mIndexPlayer2).onButtonClicked();
                }

                if (_playerTwoSelect != null)
                {
                    CAudioManager.Inst.playSfx("confirm2", _playerTwoSelect);
                }
                else
                {
                    if (_playerOneSelect != null)
                    {
                        CAudioManager.Inst.playSfx("confirm1", _playerOneSelect);
                    }
                }
            }
            else
            {
                if (mIndexPlayer1 >= 0 && mIndexPlayer1 <= mGrid.getButtonCount())
                {
                    mGrid.getButton(mIndexPlayer1).onButtonClicked();
                }

                if (_playerOneSelect != null)
                {
                    CAudioManager.Inst.playSfx("confirm1", _playerOneSelect);
                }
            }
        }
    }

    public virtual void action2Player2()
    {
        if (mGrid != null)
        {
            if (mInputType == TYPE_TWO_PLAYERS)
            {
                if (mIndexPlayer2 >= 0 && mIndexPlayer2 <= mGrid.getButtonCount())
                {
                    mGrid.getButton(mIndexPlayer2).onButtonUnclicked();
                }

                if (_playerTwoDeselect != null)
                {
                    CAudioManager.Inst.playSfx("unconfirm2", _playerTwoDeselect);
                }
                else
                {
                    if (_playerOneDeselect != null)
                    {
                        CAudioManager.Inst.playSfx("unconfirm1", _playerOneDeselect);
                    }
                }
            }
            else
            {
                if (mIndexPlayer1 >= 0 && mIndexPlayer1 <= mGrid.getButtonCount())
                {
                    mGrid.getButton(mIndexPlayer1).onButtonUnclicked();
                }

                if (_playerOneDeselect != null)
                {
                    CAudioManager.Inst.playSfx("unconfirm1", _playerOneDeselect);
                }
            }
        }

        
    }

    public void setBackgroundImage(Sprite aImage)
    {
        if (_background != null)
        {
            _background.sprite = aImage;
        }
    }

    public void notifyRemovedDevice(InputDevice aDevice)
    {
        if (mPlayerOne == aDevice)
        {
            mPlayerOne = null;

            setPlayerControllers(mPlayerOne, mPlayerTwo);
        }
        else if (mPlayerTwo == aDevice)
        {
            mPlayerTwo = null;

            setPlayerControllers(mPlayerOne, mPlayerTwo);
        }
    }

    virtual public void onButtonBack()
    {
    }
}
