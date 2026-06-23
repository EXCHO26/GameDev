using UnityEngine;

public class MimicController : EnemyController
{
    [Header("Mimic Settings")]
    [SerializeField] private float agroDistance = 5f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float wakeUpDelay = 1f; 

    private bool isAgroed = false;
    private float lastAttackTime = 0f;

    private float wakeUpEndTime = 0f; 

    protected override void UpdateMovementAnimation(bool isMoving)
    {
        m_animator.SetBool("Moving", isMoving);
    }

    private void Update()
    {
        if (m_playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, m_playerTransform.position);

        if (!isAgroed)
        {
            if (distanceToPlayer <= agroDistance)
            {
                WakeUp();
            }
            
            ApplyMovement(Vector2.zero); 
            return;
        }

        if (Time.time < wakeUpEndTime)
        {
            ApplyMovement(Vector2.zero); 
            return; 
        }

        if (distanceToPlayer <= attackDistance)
        {
            ApplyMovement(Vector2.zero);
            TryAttack();
        }
        else
        {
            m_animator.SetInteger("AttackIndex", 0);
            
            Vector2 directionToPlayer = (m_playerTransform.position - transform.position).normalized;
            ApplyMovement(directionToPlayer);
        }
    }

    private void WakeUp()
    {
        isAgroed = true;
        m_animator.SetTrigger("Agro");
        
        wakeUpEndTime = Time.time + wakeUpDelay;
    }

    private void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            int randomAttack = Random.Range(1, 3);
            m_animator.SetInteger("AttackIndex", randomAttack);
            m_animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }
}