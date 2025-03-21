using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    public int maxPlayerHealth;
    private int currentPlayerHealth;

    public int maxPlayerStamina;
    private int currentPlayerStamina;

    public GameObject playerHealthBar;
    public GameObject playerStaminaBar;

    public Volume postProcessingVolume;
    
    private int currentHealthWidth;
    private int currentStaminaWidth;

    private int maxHealthWidth;
    private int maxStaminaWidth;

    private float targetHealthWidth;
    public float lerpSpeed = .1f;

    private RectTransform healthRT;

    private float intensityVelocity = 0f;

    public TextMeshProUGUI gameOverText;

    bool gameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        currentPlayerStamina = maxPlayerStamina;

        maxHealthWidth = Mathf.CeilToInt(playerHealthBar.GetComponent<RectTransform>().sizeDelta.x);
        maxStaminaWidth = Mathf.CeilToInt(playerStaminaBar.GetComponent<RectTransform>().sizeDelta.x);

        targetHealthWidth = maxHealthWidth;
        currentStaminaWidth = maxStaminaWidth;

        healthRT = playerHealthBar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        UpdatePlayerHealthBar();

        UpdatePostProcessing();
    }

    public void TakeDamage(int damage)
    {
        currentPlayerHealth = Math.Max(0, currentPlayerHealth - damage);

        targetHealthWidth = ((float) currentPlayerHealth / maxPlayerHealth) * maxHealthWidth;

        postProcessingVolume.weight = 1f;

        if (currentPlayerHealth <= 0)
        {
            gameOver = true;
            gameOverText.gameObject.SetActive(true);
        }
    }

    void UpdatePlayerHealthBar()
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

    void UpdatePostProcessing()
    {
        if (postProcessingVolume.weight > 0.01f)
        {
            postProcessingVolume.weight = Mathf.SmoothDamp(postProcessingVolume.weight, 0.01f, ref intensityVelocity, .8f);

            if (Mathf.Abs(postProcessingVolume.weight - 0) < 0.02f) // Will never stop updating without this
            { 
                postProcessingVolume.weight = 0.01f;
            }
        }
    }
}
