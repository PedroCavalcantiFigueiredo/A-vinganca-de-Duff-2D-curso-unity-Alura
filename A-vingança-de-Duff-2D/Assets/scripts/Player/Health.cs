using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    
    // Adicionamos [SerializeField] para que possamos VER a vida no Inspector enquanto o jogo roda
    [SerializeField] private int currentHealth;

    public int CurrentHealth => currentHealth;

    public event Action OnDead;
    public event Action OnHurt;

    private void Awake()
    {
        currentHealth = maxLives;
    }

    public void TakeDamage(int amount = 1)
    {
        if (currentHealth <= 0) return;

        // --- LINHAS DE DEBUG ADICIONADAS ---
        Debug.Log(gameObject.name + " foi atingido! Dano recebido: " + amount);

        currentHealth -= amount;
        
        Debug.Log("Vida atual de " + gameObject.name + ": " + currentHealth);
        // --- FIM DAS LINHAS DE DEBUG ---

        HandleDamageTaken();
    }

    private void HandleDamageTaken()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDead?.Invoke();
        }
        else
        {
            OnHurt?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxLives);
    }
}