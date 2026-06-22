using UnityEngine;
using UnityEngine.UI;

public class VignetteController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image vignetteImage;
    [SerializeField] private float threshold = 0.25f;
    [SerializeField] private float speed = 3f;
    
    private bool isLowHealth = false;

    private void OnEnable()
    {
        if (playerHealth)
        {
            playerHealth.onHealthChanged.AddListener(CheckHealth);
        }
    }

    private void OnDisable()
    {
        if (playerHealth)
        {
            playerHealth.onHealthChanged.RemoveListener(CheckHealth);
        }  
    }

    private void CheckHealth(int current, int max)
    {
        float healthPercent = (float)current / max;
        isLowHealth = healthPercent <= threshold && current > 0;
        
        vignetteImage.enabled = isLowHealth;
    }

    private void Update()
    {
        if (isLowHealth)
        {
            ApplyVignetteEffect();
        }
    }

    private void ApplyVignetteEffect()
    {
        float pulse = (Mathf.Sin(Time.time * speed) + 1f) / 2f; 
        float minAlpha = 0.05f; 
        float maxAlpha = 0.1f;  
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, pulse);

        Color c = vignetteImage.color;
        c.a = alpha;
        vignetteImage.color = c;
    }
}
