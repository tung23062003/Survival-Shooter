using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50.0f;

    private float currentHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDead;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void UpdateHealth(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            OnDead?.Invoke();
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
