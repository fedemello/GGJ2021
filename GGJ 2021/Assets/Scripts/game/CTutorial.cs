using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTutorial : MonoBehaviour
{

    public static int _STATE_TEXT_DISABLED = 0;
    public static int _STATE_BANSHEE_TEXT = 1;
    public static int _STATE_NOSFERATU_TEXT = 2;
    public static int _STATE_ZOMBIEBOY_TEXT = 3;

    public GameObject _bansheeTextObjetct;
    public GameObject _nosferatuTextObject;
    public GameObject _zombieboyTextObject;

    public static CTutorial Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("Tutorial").AddComponent<CTutorial>();
            return _inst;
        }
    }

    private static CTutorial _inst;

    public int _state;

    public bool _tutorialEnabled;

    private void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    public void SetState(int aState)
    {
        _state = aState;

        if(_state == _STATE_TEXT_DISABLED)
        {
            _bansheeTextObjetct.SetActive(false);
            _nosferatuTextObject.SetActive(false);
            _zombieboyTextObject.SetActive(false);
            //Time.timeScale = 1;

        }
        else if(_state == _STATE_BANSHEE_TEXT)
        {
            _bansheeTextObjetct.SetActive(true);
            _nosferatuTextObject.SetActive(false);
            _zombieboyTextObject.SetActive(false);

            //Time.timeScale = 0;

        }
        else if (_state == _STATE_NOSFERATU_TEXT)
        {
            _bansheeTextObjetct.SetActive(false);
            _nosferatuTextObject.SetActive(true);
            _zombieboyTextObject.SetActive(false);
            //Time.timeScale = 0;

        }
        else if (_state == _STATE_ZOMBIEBOY_TEXT)
        {
            _bansheeTextObjetct.SetActive(false);
            _nosferatuTextObject.SetActive(false);
            _zombieboyTextObject.SetActive(true);
            //Time.timeScale = 0;

        }



    }


    public void setTutorialEnable(bool enable)
    {
        _tutorialEnabled = enable;
    }

    public bool getTutorialEnable()
    {
        return _tutorialEnabled;
    }


}
