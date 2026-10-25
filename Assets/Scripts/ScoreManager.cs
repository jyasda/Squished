using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager inst;

    public TMP_Text scoreText;
    public TMP_Text bestScoreText;
    public GameObject gameOverPanel;

    int score;
    int best;

    void Awake()
    {
        inst = this;
        best = PlayerPrefs.GetInt("Highscore", 0);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt("Highscore", best);
        }
        UpdateUI();
    }

    public void GameOver()
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score: " + score;
        if (bestScoreText) bestScoreText.text = "Best Score: " + best;
    }
}