using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameStatsManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI causeText;
    [SerializeField] private TextMeshProUGUI restartText;

    [SerializeField] private GameObject gameOverPanel;

    [Header("Scoring")]
    [SerializeField] private float scorePerSecond = 10f;

    private float elapsedTime = 0f;
    private float currentScore = 0f;
    private bool isGameActive = true;
    private bool isGameOver = false;

    public static GameStatsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        gameOverPanel.SetActive(false);
        recordText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isGameActive)
        {
            elapsedTime += Time.deltaTime;
            currentScore += scorePerSecond * Time.deltaTime * ScoreMultiplierManager.Instance.GetMultiplier();
            UpdateUI();
        }
        else if (isGameOver)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                RestartGame();
            }
        }
    }

    private void UpdateUI()
    {
        if (timeText != null)
        {
            timeText.text = $"□ Time: {elapsedTime:F1}s";
        }

        if (scoreText != null)
        {
            scoreText.text = $"□ Score: {Mathf.FloorToInt(currentScore)}";
        }
    }

    public void EndGame(string causa)
    {
        isGameActive = false;
        isGameOver = true;

        int finalScore = GetFinalScore();
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        gameOverPanel.SetActive(true);

        scoreText.text = $"□ Score: {finalScore}";

        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save();
            recordText.text = "■ NEW RECORD!";
            recordText.gameObject.SetActive(true);
        }
        else
        {
            recordText.text = $"■ Record: {highScore}";
            recordText.gameObject.SetActive(true);
        }

        causeText.text = $"Cause of Death: {causa}";
        causeText.gameObject.SetActive(true);

        restartText.text = "Press any key to restart";
    }

    public int GetFinalScore()
    {
        return Mathf.FloorToInt(currentScore);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    private void RestartGame()
    {
        gameOverPanel.SetActive(false);
        recordText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}