using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected float m_speed = 2.0f;

    protected Rigidbody2D m_body2d;
    protected Animator m_animator;

    protected virtual void Start()
    {
        m_body2d = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    public void ModifySpeed(float amount)
    {
        m_speed += amount;
    }
    protected void ApplyMovement(Vector2 direction)
    {
        m_body2d.linearVelocity = direction * m_speed;

        if (direction.x > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1.0f);
        }
        else if (direction.x < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1.0f);
        }

        bool isMoving = direction.magnitude > Mathf.Epsilon;
        UpdateMovementAnimation(isMoving);
    }
    protected abstract void UpdateMovementAnimation(bool isMoving);
}
