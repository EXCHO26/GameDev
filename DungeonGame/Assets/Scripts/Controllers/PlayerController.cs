using UnityEngine;
public class PlayerController : EntityController
{
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;

    protected override void UpdateMovementAnimation(bool isMoving)
    {
        if (isMoving)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0) m_animator.SetInteger("AnimState", 0);
        }
    }

    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(inputX, inputY).normalized;

        bool isAttacking = m_timeSinceAttack < 0.25f;
        bool isBlocking = m_animator.GetBool("IdleBlock");

        if (isAttacking || isBlocking)
        {
            ApplyMovement(Vector2.zero);
        }

        else
        {
            ApplyMovement(movement);
        }

        HandleCombatInput();
    }

    void HandleCombatInput()
    {
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f)
        {
            m_currentAttack = (m_timeSinceAttack > 1.0f || m_currentAttack >= 3) ? 1 : m_currentAttack + 1;
            m_animator.SetTrigger("Attack" + m_currentAttack);
            m_timeSinceAttack = 0.0f;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }
        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);
    }
}