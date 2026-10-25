using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject gameOverCanvas; 

    private bool isDead = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverCanvas) gameOverCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TriggerGameOver()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player squished!");
        Time.timeScale = 0f; 

        if (gameOverCanvas) gameOverCanvas.SetActive(true);
    }

    
    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}