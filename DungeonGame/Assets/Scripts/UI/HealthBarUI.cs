using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthFill;
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthFill == null) return;

        healthFill.fillAmount = Mathf.Clamp01((float)currentHealth / (float)maxHealth);
    }
}