using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �V�[���̍ēǂݍ��݂ɕK�v

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
        Time.timeScale = 1; // �Q�[�����Ԃ�i�߂�
    }

    void Update()
    {
        if (!isGameOver)
        {
            // �i�s�������v�Z
            distance = startY - lure.position.y;
            scoreText.text = "Distance: " + distance.ToString("F1") + "m";
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0; // �Q�[�����~
        gameOverPanel.SetActive(true);
    }

    public void SubmitScore()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Anonymous";
        }

        // PlayerPrefs���g���ă��[�J���ɃX�R�A��ۑ�
        PlayerPrefs.SetFloat("BestScore", distance);
        PlayerPrefs.SetString("BestPlayer", playerName);
        PlayerPrefs.Save();

        // �����Ń��[�_�[�{�[�h�\���V�[���Ɉړ�����Ȃǂ̏���
        Debug.Log("Score Submitted: " + playerName + " - " + distance);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}