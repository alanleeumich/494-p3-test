using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ScoreKeeper : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI gameOverText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    int health = 10;
    int score = 0;

    bool gameOver = false;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void UpdateScore(int amount)
    {
        score += amount;

        scoreText.text = "Score: " + score.ToString();
        if(score > 3)
        {
            //StartCoroutine(GameReset());
        }
    }

    public void UpdateHealth(int amount)
    {
        health += amount;
        if (health <= 0)
        {
            Time.timeScale = 0;
            gameOver = true;
            gameOverText.gameObject.SetActive(true);
        }

        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + health.ToString();
    }

    //private IEnumerator GameReset()
    //{
    //    scoreText.text = "Enemy defetaed!!! You Win!";
    //    yield return new WaitForSeconds(5f);
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    //}
}
