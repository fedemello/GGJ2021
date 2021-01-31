using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CScoreManager : MonoBehaviour
{
    public TextMeshProUGUI _textScore;

    public int _basicPoints;
    public int _comboPoints;
    public int _currentCombo = 0;
    private int _endCombo = 100;
    public int _score;
    private int _maxScore;


    private int mCurrentClassification = -1;

    public List<Sprite> mScores = new List<Sprite>();

    public SpriteRenderer _scoreText;

    public CEnemyScroller mScroller;

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
        int clas = Classification();



        if (mCurrentClassification != clas)
        {
            _scoreText.sprite = mScores[clas];

            mCurrentClassification = clas;
        }
        //_textScore.text = ("Score: " + _score.ToString());

        //if(Input.GetKeyDown(KeyCode.W))
        //{
        //    AddToScore();
        //}
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

    public int Classification()
    {
        int clasy = 0;

        _maxScore = mScroller._cantidadMaxSpawn * 50;

        float percentaje = (_score * 100)/_maxScore;

        if (_endCombo == _maxScore)
        {
            clasy = 5;
        }
        
        else if (percentaje >= 95)
        {
            clasy = 4;
        } 
        
        else if (percentaje >= 90)
        {
            clasy = 3;
        }

        else if (percentaje >= 80)
        {
            clasy = 2;
        }

        else if (percentaje >= 70)
        {
            clasy = 1;
        }

        else if (percentaje >= 60)
        {
            clasy = 0;
        }

        return clasy;
    }

    public int getMaxScore()
    {
        return _maxScore;
    }
}
