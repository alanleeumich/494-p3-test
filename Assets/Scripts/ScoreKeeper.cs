using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class ScoreKeeper : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI gameOverText; //ignore for now
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
        if(score >= 3)
        {
            StartCoroutine(GameReset(true));
        }
    }

    public void UpdateHealth(int amount)
    {
        health += amount;
        if (health <= 0)
        {
            gameOver = true;
            StartCoroutine(GameReset(false));
        }

        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + health.ToString();
    }

    private IEnumerator GameReset(bool you_won)
    {
        if (you_won)
        {
            scoreText.text = "Enemy defeated!!! You Win!";
            GameObject.Find("Enemy").SetActive(false);
        }
        if (!you_won)
        {
            scoreText.text = "You Died! Game Over";
            GameObject.Find("Player").SetActive(false);
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
