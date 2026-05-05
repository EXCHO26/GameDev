using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // -- Handle input and movement --
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        // Move in 2.5D space (X and Y)
        Vector2 movement = new Vector2(inputX, inputY).normalized;
        m_body2d.linearVelocity = movement * m_speed;

        // -- Handle Animations --

        //Death
        if (Input.GetKeyDown("e"))
        {
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
        {
            m_animator.SetTrigger("Hurt");
        }

        //Attack
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f)
        {
            m_currentAttack++;

            if (m_currentAttack > 3)
                m_currentAttack = 1;

            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            m_animator.SetTrigger("Attack" + m_currentAttack);
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1))
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        //Run/Idle Animation State
        if (movement.magnitude > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if(m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }
}