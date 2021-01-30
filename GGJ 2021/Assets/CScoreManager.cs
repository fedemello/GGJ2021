using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CScoreManager : MonoBehaviour
{
    public Text _textScore;

    public int _basicPoints;
    public int _comboPoints;
    public int _currentCombo = 0;
    private int _endCombo = 0;
    public int _score;
    private int _maxScore;

    public static CScoreManager Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("Score Manager").AddComponent<CScoreManager>();
            return _inst;
        }
    }
    private static CScoreManager _inst;
    
    void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;
    }

    private void Update() 
    {
        _textScore.text = ("Score: " + _score.ToString());

        if(Input.GetKeyDown(KeyCode.W))
        {
            AddToScore();
        }
    }

    public void AddToScore()
    {
        _score += (_basicPoints + (_comboPoints * _currentCombo));

        _currentCombo += 1;
    }

    public void BrokeCombo()
    {
        _currentCombo = 0;
    }

    public void EndCombo()
    {
        _endCombo = _currentCombo;
    }

    public string Classification()
    {
        string clasy = null;

        float percentaje = (_endCombo * 100)/_maxScore;

        if (_endCombo == _maxScore)
        {
            clasy = "S++";
        }
        
        else if (percentaje >= 95)
        {
            clasy = "S";
        } 
        
        else if (percentaje >= 90)
        {
            clasy = "A";
        }

        else if (percentaje >= 80)
        {
            clasy = "B";
        }

        else if (percentaje >= 70)
        {
            clasy = "C";
        }

        else if (percentaje >= 60)
        {
            clasy = "D";
        }

        return clasy;
    }
}
