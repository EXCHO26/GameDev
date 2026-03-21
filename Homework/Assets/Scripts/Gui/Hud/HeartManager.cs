using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public List<Image> heartImages; 

    private int lastKnownHealth;

    void Start()
    {
        lastKnownHealth = playerHealth.health;
    }

    void Update()
    {
        if (playerHealth.health != lastKnownHealth)
        {
            UpdateHeartUI();
            lastKnownHealth = playerHealth.health;
        }
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < playerHealth.health)
            {
                heartImages[i].sprite = fullHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }
}