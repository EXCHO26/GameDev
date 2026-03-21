using UnityEngine;

public class DisapearOnPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Item is picked up");

            Destroy(gameObject);
        }
    }
}
