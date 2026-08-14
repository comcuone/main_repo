using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] private int maxHealth = 500;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text hpText;

    private int currentHealth;

    public bool IsDead { get; private set; } = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    
    //isTrigger체크되어있을때 충돌처리
    // private void OnTriggerEnter2D(Collider2D other) {
    //     if (other.gameObject.tag == "Enemy")
    //     {
    //         TakeDamage(20);
    //         Destroy(other.gameObject);
    //     }
    //     else if (other.gameObject.tag == "Heal")
    //     {
    //         TakeDamage(-50);
    //         Destroy(other.gameObject);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("충돌 : " + other.name);

        if (other.CompareTag("Enemy"))
        {
            TakeDamage(90);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Heal"))
        {
            Debug.Log("힐!");
            TakeDamage(-50);
            Destroy(other.gameObject);
        }
    }
    //얘는isTrigger꺼져있을때(>물리적충돌있을때) 충돌처리
    // private void OnCollisionEnter2D(Collision2D other) {
        
    // }
    //힐템

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;
            
        if (currentHealth > maxHealth)
        currentHealth = maxHealth;

        UpdateHealthUI();

        Debug.Log($"플레이어 체력 : {currentHealth}/{maxHealth}");

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

        Debug.Log("플레이어 사망");
    }
}