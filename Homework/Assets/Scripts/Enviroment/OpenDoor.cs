using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public PlayerKeys playerKeys;      
    public GameObject closedDoor;  

    void Update()
    {
        if (playerKeys.keysCollected >= 3)
        {
            closedDoor.SetActive(false);
        }
    }
}
