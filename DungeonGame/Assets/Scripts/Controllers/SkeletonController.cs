using UnityEngine;

public class SkeletonController : EntityController
{
    public Transform player;
    public float chaseRadius = 5f;
    public float attackRadius = 1.2f;

    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    private int currentAttackIndex = 0;

    protected override void UpdateMovementAnimation(bool isMoving)
    {
        m_animator.SetBool("Moving", isMoving);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        float yDifference = Mathf.Abs(transform.position.y - player.position.y);
        float maxVerticalAlignment = 0.3f;

        if (distance < chaseRadius)
        {
            if (distance > attackRadius && yDifference > maxVerticalAlignment)
            {
                Vector2 direction = (player.position - transform.position).normalized;
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
