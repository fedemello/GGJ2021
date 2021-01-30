using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class CLevelManager : MonoBehaviour
{
    public static CLevelManager Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("Level Manager").AddComponent<CLevelManager>();
            return _inst;
        }
    }
    private static CLevelManager _inst;

    private CStateBase mCurrentState;

    private Coroutine _activeCoroutine;

    public static int PLAYER_NONE = 0;
    public static int PLAYER_ONE = 1;
    public static int PLAYER_TWO = 2;

    private InputDevice mPlayerOne;
    private InputDevice mPlayerTwo;

    private bool mLoadedState = false;

    public void Start()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Create the local data manager.
        if (CLocalData.inst() == null)
        {
            new CLocalData();
        }

        // Create the tournament manager.
        //_ = CTournamentManager.Inst;

        // Create the player data manager.
        //_ = CPlayerDataManager.Inst;

        // Create the transition manager.
        _ = CTransitionManager.Inst;

        // Create the input manager.
        _ = CInputManager.Inst;

        _inst = this;

        findCurrentState();

        mCurrentState.init();

        updatePlayerControllers();

        mCurrentState.setPlayerControllers(mPlayerOne, mPlayerTwo);

        mLoadedState = true;
    }

    public void Update()
    {
        if (mCurrentState != null && !isTransitioning())
        {
            mCurrentState.update();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CInputManager.Inst.logDevices();
        }
    }

    public bool isTransitioning()
    {
        return _activeCoroutine != null;
    }

    public float getDeltaTime()
    {
        return Time.deltaTime;
    }

    // TODO: si llegamos a precisar estos load, implementar a la lógica actual.
    //public void LoadScreen(string name)
    //{
    //    SceneManager.LoadSceneAsync(name);
    //}

    //public void LoadSceneAdditive(string name)
    //{
    //    SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    //}

    public void loadScene(string aScene, bool aFade = true)
    {

        if (_activeCoroutine == null)
        {
            Debug.Log("loading scene: " + aScene);
            _activeCoroutine = StartCoroutine(transitionLevel(aScene, aFade));
        }
    }

    public IEnumerator transitionLevel(string aScene, bool aFade)
    {
        mLoadedState = false;

        if (aFade)
        {
            CTransitionManager.Inst.CreateTransition("Fade");

            while (CTransitionManager.Inst.IsScreenCovered() != true)
            {
                yield return null; //esperar 1 frame
            }
        }

        //CAudioManager.Inst.stopAllSfx();

        if (mCurrentState != null)
        {
            // Exit previous state.
            mCurrentState.exitState();

            mCurrentState = null;
        }

        // Load new scene.
        SceneManager.LoadScene(aScene);

        yield return null;

        findCurrentState();

        mCurrentState.init();

        updatePlayerControllers();

        mCurrentState.setPlayerControllers(mPlayerOne, mPlayerTwo);

        _activeCoroutine = null;

        mLoadedState = true;

        yield return null;
    }

    /// <summary>
    /// Returns the current state
    /// </summary>
    /// <returns></returns>
    public CStateBase getCurrentState()
    {
        if (mCurrentState == null)
        {
            findCurrentState();
        }

        return mCurrentState;
    }

    private void findCurrentState()
    {
        //Debug.Log("searching current state");
        mCurrentState = FindObjectOfType<CStateBase>();

        //bool notNull = mCurrentState != null;

        //Debug.Log("current state: " + notNull);
    }

    /// <summary>
    /// Makes sure we get the player controllers.
    /// </summary>
    public void updatePlayerControllers()
    {
        if (mPlayerOne == null || !mPlayerOne.IsAttached)
        {
            mPlayerOne = CInputManager.Inst.getPlayerControl();
            //Debug.Log("updating player one controller: " + (mPlayerOne != null).ToString());
        }

        if (mPlayerTwo == null || !mPlayerTwo.IsAttached)
        {
            mPlayerTwo = CInputManager.Inst.getPlayerControl(false);
            //Debug.Log("updating player two controller: " + (mPlayerTwo != null).ToString());

        }
    }

    public void setPlayerControllers(InputDevice aPlayerOne, InputDevice aPlayerTwo)
    {
        // This will already be done automatically.
        if (!mLoadedState)
        {
            return;
        }

        mPlayerOne = aPlayerOne;
        mPlayerTwo = aPlayerTwo;

        if (mCurrentState != null)
        {
            mCurrentState.setPlayerControllers(mPlayerOne, mPlayerTwo);
        }
    }
}