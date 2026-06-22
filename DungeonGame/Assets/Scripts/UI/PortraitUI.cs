using UnityEngine;
using UnityEngine.UI;

public class PortraitUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    private Image uiImage;

    void Start()
    {
        uiImage = GetComponent<Image>();
    }

    void LateUpdate()
    {
        if (playerSpriteRenderer != null && uiImage != null)
        {
            uiImage.sprite = playerSpriteRenderer.sprite;
        }
    }
}