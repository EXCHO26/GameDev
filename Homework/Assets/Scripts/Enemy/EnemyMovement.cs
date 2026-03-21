using UnityEngine;

public class EnemySentryAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 6f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private int direction = 1;
    private bool isAggressive = false;

    [Header("Sensor Check")]
    public Collider2D floorSensor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            isAggressive = true;
        }
        else
        {
            isAggressive = false;
        }

        animator.SetBool("Agro", isAggressive);
    }

    void FixedUpdate()
    {
        if (isAggressive)
        {
            if (!floorSensor.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Flip();
            }

            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
        else 
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Flip()
    {
        direction *= -1;
        transform.localScale = new Vector3(direction, 1, 1);
        
        transform.position += new Vector3(direction * 0.2f, 0, 0);
    }
}