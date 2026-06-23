using UnityEngine;

public class SkeletonController : EnemyController
{
    private float chaseRadius = 5f;
    private float attackRadius = 1.2f;

    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    private int currentAttackIndex = 0;

    protected override void UpdateMovementAnimation(bool isMoving)
    {
        m_animator.SetBool("Moving", isMoving);
    }

    void Update()
    {
        if (m_playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, m_playerTransform.position);
        float yDifference = Mathf.Abs(transform.position.y - m_playerTransform.position.y);
        float maxVerticalAlignment = 0.3f;

        if (distance < chaseRadius)
        {
            if (distance > attackRadius && yDifference > maxVerticalAlignment)
            {
                Vector2 direction = (m_playerTransform.position - transform.position).normalized;
                ApplyMovement(direction);
            }
            else
            {
                ApplyMovement(Vector2.zero);
                TryAttack();
            }
        }
        else
        {
            ApplyMovement(Vector2.zero);
        }
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            m_animator.SetInteger("AttackIndex", currentAttackIndex);
            m_animator.SetTrigger("Attack");
            
            currentAttackIndex = (currentAttackIndex + 1) % 2;
            lastAttackTime = Time.time;
        }
    }
}