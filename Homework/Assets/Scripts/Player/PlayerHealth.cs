using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int health = 3;

    [Header("Visual Feedback")]
    public Animator animator;

    [Header("Low Health Visuals")]
    public UnityEngine.UI.Image redVignette;
    public float pulseSpeed = 3f;
    public float maxOpacity = 0.5f;

    void Update()
    {
        CheckHealth();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyKnockback knockback = other.GetComponent<EnemyKnockback>();

            float forceToApply = 20f;

            if (knockback != null)
            {
                forceToApply = knockback.knockbackPower;
            }

            TakeDamage();
            PushBack(other.transform.position, forceToApply);
        }
    }

    void TakeDamage()
    {
        if (health > 0)
        {
            health--;
        }

        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
    }

    void PushBack(Vector2 enemyPosition, float knockbackPower)
    {
        float dir = transform.position.x < enemyPosition.x ? -1 : 1;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(dir * knockbackPower * 10f, 5f), ForceMode2D.Impulse);
    }

    void CheckHealth()
    {
        if (redVignette != null)
        {
            if (health <= 1)
            {
                float alpha = Mathf.PingPong(Time.time * pulseSpeed, maxOpacity);
                redVignette.color = new Color(1, 0, 0, alpha);
            }
            else
            {
                redVignette.color = new Color(1, 0, 0, 0);
            }
        }
    }
}
