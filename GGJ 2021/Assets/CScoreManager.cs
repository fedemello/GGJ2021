using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CScoreManager : MonoBehaviour
{
    public int _score;
    private int _minScore;
    private int _maxScore;

    private int mCurrentClassification = -1;

    public List<Sprite> mScores = new List<Sprite>();

    public SpriteRenderer _scoreText;

    public CEnemyScroller mScroller;

    public CPlayerLifebar _playerScore;

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

    public void resetScore()
    {
        _score = 0;

        _maxScore = mScroller._cantidadMaxSpawn * 50;

        _minScore = _maxScore / 2;
    }

    private void Update() 
    {


        
        //_textScore.text = ("Score: " + _score.ToString());

        //if(Input.GetKeyDown(KeyCode.W))
        //{
        //    AddToScore();
        //}
    }

    public void AddToScore(int score)
    {
        _score += score;

        updateBarAndClassification();
    }

    public void updateBarAndClassification()
    {
        // inicio min score final max score
        // de 0 a max score. 

        // Total de enemies.
        int enemiesRemaining = mScroller._cantidadMaxSpawn - mScroller._currentCantidadSpawn + CEnemyManager.Inst.cantEnemies();

        int posibleScore = enemiesRemaining * 50;

        float val = CMath.lerp(_minScore, _maxScore, 0, 1, _score + posibleScore);

        Debug.Log("val: " + val);

        if (val <= 0)
        {
            Debug.Log("lost!");
            CSingingStage state = CLevelManager.Inst.getCurrentState() as CSingingStage;

            if (state != null)
            {
                state.notifyLoss();
            }
        }

        //  _maxScore
        _playerScore.setCurrentPercent(val);

        int clas = getClassification(val);

        if (mCurrentClassification != clas)
        {
            _scoreText.sprite = mScores[clas];

            mCurrentClassification = clas;
        }
    }

    public int getClassification(float val)
    {
        //_maxScore = mScroller._cantidadMaxSpawn * 50;

        if (val <= 0.16f)
        {
            return 0;
        }
        else if (val <= 0.27f)
        {
            return 1;
        }
        else if (val <= 0.5f)
        {
            return 2;
        }
        else if (val <= 0.66f)
        {
            return 3;
        }
        else if (val <= 0.83f)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    public int getMaxScore()
    {
        return _maxScore;
    }
}
