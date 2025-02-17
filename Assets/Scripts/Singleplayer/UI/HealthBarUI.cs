using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;

    private Health health;

    private void Awake()
    {
        health = GetComponentInParent<Health>();

        if (health != null)
            health.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
}
