using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Bson;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] Stairs;
    public bool[] isTurn;


    private enum State { Start, Left, Right };
    private State state;
    private Vector3 oldPosition;
    private int kiwiScore = 0;

    public GameObject _gameOver;
    public TMP_Text nowScoreText, bestScoreText, scoreText, kiwiScoreText;
    public int maxScore = 0;
    public int nowScore = 0;
    

    public UnityEngine.UI.Image _gauge;
    public bool _gaugeStart = false;
    float gaugeRedcutionRate = 0.0015f;


    void Awake()
    {
        Instance = this;
        Init();
        InitStairs();
        StartCoroutine("CheckGauge");
        GaugeReduce();
        LoadKiwiScore();
        
    }

   
   
    public void Init()
    {
        state = State.Start;
        oldPosition = Vector3.zero;
        isTurn = new bool[ Stairs.Length];
        for (int i = 0; i < Stairs.Length; i++)
        {
            Stairs[i].transform.position = Vector3.zero;
            isTurn[i] = false;
        }
        nowScore = 0;
        scoreText.text = nowScore.ToString();
        _gameOver.SetActive(false);
        _gauge.fillAmount = 1f;
        gaugeRedcutionRate = 0.0015f;

    }
    public void InitStairs()
    {
        for (int i = 0; i < Stairs.Length; i++)
        {
            switch (state)
            {
                case State.Start:
                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, -0.1f, 0);
                    state = State.Right;
                    break;
                case State.Left:
                    Stairs[i].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                    isTurn[i] = true;
                    break;

                case State.Right:
                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                    isTurn[i] = false;
                    break;
            }
            oldPosition = Stairs[i].transform.position;

            if (i != 0)
            {
                int ran = Random.Range(0, 5);
                if (ran < 2 && i < Stairs.Length - 1)
                {
                    state = state == State.Left ? State.Right : State.Left;
                }
            }
        }
    }

    public void SpawnStair(int count)
    {
        int ran = Random.Range(0, 5);
        if (ran < 2)
        {
            state = state == State.Left ? State.Right : State.Left;

        }
        switch (state)
        {

            case State.Left:
                Stairs[count].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                isTurn[count] = true;
                break;

            case State.Right:
                Stairs[count].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                isTurn[count] = false;
                break;
        }
        oldPosition = Stairs[count].transform.position;
    }

    public void AddScore()
    {
        nowScore++;
        scoreText.text = nowScore.ToString();

    }

    public void GaugeIncrease()
    {
        _gauge.fillAmount += 0.05f;
        if(_gauge.fillAmount > 1f) _gauge.fillAmount = 1f;
    }

    public void GaugeReduce()
    {
        if (_gaugeStart)
        {
            if (nowScore > 30) gaugeRedcutionRate = 0.0033f;
            if (nowScore > 60) gaugeRedcutionRate = 0.0037f;
            if (nowScore > 100) gaugeRedcutionRate = 0.0043f;
            if (nowScore > 150) gaugeRedcutionRate = 0.005f;
            if (nowScore > 200) gaugeRedcutionRate = 0.005f;
            if (nowScore > 300) gaugeRedcutionRate = 0.0065f;
            if (nowScore > 400) gaugeRedcutionRate = 0.0075f;
            _gauge.fillAmount -= gaugeRedcutionRate;
            if(_gauge.fillAmount <= 0)
            {
                _gauge.fillAmount = 0;
                GameOver();
            }
        }
        Invoke("GaugeReduce", 0.02f);
    }

    IEnumerator CheckGauge()
    {
        while (_gauge.fillAmount != 0)
        {
            yield return new WaitForSeconds(0.4f);
        }
        GameOver();
    }

    public void GameOver()
    {
        CancelInvoke();
        StartCoroutine(ShowGameOver());
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1f);
        _gameOver.SetActive(true);

        if (nowScore > maxScore)
        {
            maxScore = nowScore;
        }

        bestScoreText.text = maxScore.ToString();
        nowScoreText.text = nowScore.ToString();

    }

    public void AddKiwiScore(int amount)
    {
        kiwiScore += amount;
        PlayerPrefs.SetInt("KiwiScore", kiwiScore);
        PlayerPrefs.Save();
        UpdateKiwiScoreUI();
    }

    private void UpdateKiwiScoreUI()
    {
        kiwiScoreText.text = $":{kiwiScore}";
    }

    private void LoadKiwiScore()
    {
        kiwiScore = PlayerPrefs.GetInt("KiwiScore", 0);
        UpdateKiwiScoreUI();
    }

    

    
}
