using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeysManager : MonoBehaviour
{
    public PlayerKeys playerKeys;
    public Sprite fullKey;
    public Sprite emptyKey;
    public List<Image> keyImages; 

    private int lastKnownKeys;

    void Start()
    {
        lastKnownKeys = playerKeys.keysCollected;
    }

    void Update()
    {
        if (playerKeys.keysCollected != lastKnownKeys)
        {
            UpdateKeyUI();
            lastKnownKeys = playerKeys.keysCollected;
        }
    }

    void UpdateKeyUI()
    {
        for (int i = 0; i < keyImages.Count; i++)
        {
            if (i < playerKeys.keysCollected)
            {
                keyImages[i].sprite = fullKey;
            }
        }
    }
}