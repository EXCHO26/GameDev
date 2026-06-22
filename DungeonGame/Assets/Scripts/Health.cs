using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Events")]
    public UnityEvent<int, int> onHealthChanged; 
    public UnityEvent onHit;
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        onHealthChanged?.Invoke(currentHealth, maxHealth);
        onHit?.Invoke();

        if (currentHealth <= 0)
        {
            isDead = true;
            onDeath?.Invoke();
        }
    }
}