using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーンの再読み込みに必要

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform lure;
    public Text scoreText;
    public GameObject gameOverPanel;
    public InputField nameInputField;

    private float distance = 0f;
    private float startY;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        startY = lure.position.y;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1; // ゲーム時間を進める
    }

    void Update()
    {
        if (!isGameOver)
        {
            // 進行距離を計算
            distance = startY - lure.position.y;
            scoreText.text = "Distance: " + distance.ToString("F1") + "m";
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0; // ゲームを停止
        gameOverPanel.SetActive(true);
    }

    public void SubmitScore()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Anonymous";
        }

        // PlayerPrefsを使ってローカルにスコアを保存
        PlayerPrefs.SetFloat("BestScore", distance);
        PlayerPrefs.SetString("BestPlayer", playerName);
        PlayerPrefs.Save();

        // ここでリーダーボード表示シーンに移動するなどの処理
        Debug.Log("Score Submitted: " + playerName + " - " + distance);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}