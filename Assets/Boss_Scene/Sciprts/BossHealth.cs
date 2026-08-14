using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] private int maxHealth = 500;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text hpText;

    private int currentHealth;

    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthUI();

        Debug.Log($"보스 체력 : {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        hpText.text = $"{currentHealth} / {maxHealth} HP";
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        Debug.Log("보스 처치!");
    }
}