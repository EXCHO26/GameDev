using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image healthFill;
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth = amount;
        
        healthFill.fillAmount = currentHealth / maxHealth;
    }
}