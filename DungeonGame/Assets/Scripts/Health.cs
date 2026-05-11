using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    private HealthBarController uiHealthBar;

    public UnityEvent onHit;
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;
        uiHealthBar = FindFirstObjectByType<HealthBarController>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} health: {currentHealth}");

        if (uiHealthBar)
        {
            uiHealthBar.UpdateHealth(currentHealth);
        }

        onHit.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        
        if (TryGetComponent<EntityController>(out var controller))
        {
            controller.enabled = false;
        }
            
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;

        }
            
        onDeath.Invoke(); 

        Destroy(gameObject, 2f);
    }
}