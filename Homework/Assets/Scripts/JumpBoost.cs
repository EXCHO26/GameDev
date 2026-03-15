using UnityEngine;

public class JumpBoost : MonoBehaviour
{
    public float jumpForce = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                
            }
        }
    }
}
