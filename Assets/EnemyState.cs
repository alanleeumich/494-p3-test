using System;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EnemyState : MonoBehaviour
{
    public int maxEnemyHealth;
    private int currentEnemyHealth;

    public GameObject enemyHealthBar;

    public TextMeshProUGUI gameWinText;
    
    private int currentHealthWidth;

    private int maxHealthWidth;

    private float targetHealthWidth;
    public float lerpSpeed = 10f;

    private RectTransform healthRT;

    private float intensityVelocity = 0f;

    private bool gameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;

        maxHealthWidth = Mathf.CeilToInt(enemyHealthBar.GetComponent<RectTransform>().sizeDelta.x);

        targetHealthWidth = maxHealthWidth;

        healthRT = enemyHealthBar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        UpdateEnemyHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentEnemyHealth = Math.Max(0, currentEnemyHealth - damage);

        targetHealthWidth = ((float) currentEnemyHealth / maxEnemyHealth) * maxHealthWidth;

        if (currentEnemyHealth <= 0)
        {
            gameOver = true;
            gameWinText.gameObject.SetActive(true);
            StartCoroutine(FreezeAfterOneSecond());
        }
    }

    void UpdateEnemyHealthBar()
    {
        if (healthRT.sizeDelta.x > targetHealthWidth)
        {
            float newWidth = Mathf.Lerp(healthRT.sizeDelta.x, targetHealthWidth, Time.deltaTime * lerpSpeed);

            if (Mathf.Abs(newWidth - targetHealthWidth) <= 0.02f) // Will never stop updating without this
            {
                newWidth = targetHealthWidth;
            }
            
            healthRT.sizeDelta = new Vector2(newWidth, healthRT.sizeDelta.y);
        }   
    }

    IEnumerator FreezeAfterOneSecond()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0f;
    }
}
