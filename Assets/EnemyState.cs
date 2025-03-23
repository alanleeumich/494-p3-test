using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyState : MonoBehaviour
{
    public int maxEnemyHealth;
    private int currentEnemyHealth;

    public GameObject enemyHealthBar;
    
    private int currentHealthWidth;

    private int maxHealthWidth;

    private float targetHealthWidth;
    public float lerpSpeed = 10f;

    private RectTransform healthRT;

    private float intensityVelocity = 0f;
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

        UpdateEnemyHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentEnemyHealth = Math.Max(0, currentEnemyHealth - damage);

        targetHealthWidth = ((float) currentEnemyHealth / maxEnemyHealth) * maxHealthWidth;
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
}
