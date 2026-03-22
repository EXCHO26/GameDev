using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    public PlayerKeys playerKeys;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door") && playerKeys.keysCollected >= 3)
        {
            Debug.Log("You Compleated the Level!");
        }
    }
}
