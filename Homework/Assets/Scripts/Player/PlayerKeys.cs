using UnityEngine;

public class PlayerKeys : MonoBehaviour
{
    public int keysCollected = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            keysCollected++;
        }
    }
}
