using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityAction PreparingGame;
    public static GameManager instance;
    [SerializeField] private GameObject StartPanel,GamePlayPanel , GameOverPanel;
    private int currentScore = 0, bestScore;
    [SerializeField]
    private TextMeshProUGUI currentScoreText, currentScoreText2, bestScoreText1, bestScoreText2;
    [SerializeField] 
    private GameObject textShowPrefab;
    [SerializeField]
    private GameObject objectPool;
    public enum GameState
    {
        Prepare,
        MainGame,
        FinishGame,
    }
    private GameState _currentGameState;
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set
        {
            switch (value)
            {
                case GameState.Prepare:
                    PreparingGame?.Invoke();
                    break;
                case GameState.MainGame:
                    break;
                case GameState.FinishGame:
                    break;
                default:
                    break;
            }
            _currentGameState = value;
        }
    }
    private void Awake()
    {
        instance = this;
        _currentGameState = GameState.Prepare;
    }
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("bestScore", 0);
    }
    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Prepare:
                PrepareGame();
                break;
            case GameState.MainGame:
                if (Input.GetMouseButtonDown(0))
                {
                    currentScore += 1;
                }
                GamePlay();
                break;
            case GameState.FinishGame:
                GameOver();
                break;
            default:
                break;
        }
    }
    void PrepareGame()
    {
        currentScore = 0;
        bestScoreText1.text = "best: " + PlayerPrefs.GetInt("bestScore", 0).ToString();
        bestScoreText2.text = "best: " + PlayerPrefs.GetInt("bestScore", 0).ToString();
        StartPanel.SetActive(true);
        GameOverPanel.SetActive(false);
        GamePlayPanel.SetActive(false);
    }
    void GamePlay()
    {
        currentScoreText.text = currentScore.ToString();
        StartPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        GamePlayPanel.SetActive(true);
    }
    void GameOver()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("bestScore", bestScore);
        }
        currentScoreText2.text = currentScore.ToString();

        bestScoreText1.text = "best: " + PlayerPrefs.GetInt("bestScore", 0).ToString();
        bestScoreText2.text = "best: " + PlayerPrefs.GetInt("bestScore", 0).ToString();
        StartPanel.SetActive(false);
        GameOverPanel.SetActive(true);
        GamePlayPanel.SetActive(false);
    }
    public void StartGame()
    {
        CurrentGameState = GameState.MainGame;
    }
    public void RestartGame()
    {
       
        for (int i = 0; i < objectPool.transform.childCount; i++)
        {
            objectPool.transform.GetChild(i).gameObject.SetActive(false);
        }
        CurrentGameState = GameState.Prepare;
    }
    public void ScoreShow()
    {
        currentScore += 1;
        GameObject scoreShowText = Instantiate(textShowPrefab, Vector3.zero, Quaternion.identity);
        scoreShowText.GetComponent<Text>().text = "+1";
        scoreShowText.transform.SetParent(GameObject.Find("Canvas").transform);
        scoreShowText.transform.position = new Vector3(600f, 250f, 0f);
        Destroy(scoreShowText.gameObject, 0.4f);
    }
}
