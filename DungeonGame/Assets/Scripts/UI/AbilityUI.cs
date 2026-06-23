using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image iconImage;
    public Image cooldownOverlay;

    private ActiveAbility trackedAbility;

    public void Setup(ActiveAbility abilityToTrack)
    {
        trackedAbility = abilityToTrack;

        if (trackedAbility.ability.icon)
        {
            iconImage.sprite = trackedAbility.ability.icon;
        }

        cooldownOverlay.fillAmount = 0f;
    }

    void Update()
    {
        if (trackedAbility == null) return;

        float cooldownTotal = trackedAbility.ability.levels[trackedAbility.currentLevel - 1].cooldown;
        
        float timeSinceUsed = Time.time - trackedAbility.lastUsedTime;

        if (timeSinceUsed < cooldownTotal)
        {
            float timeLeft = cooldownTotal - timeSinceUsed;
        
            cooldownOverlay.fillAmount = timeLeft / cooldownTotal;
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;
        }
    }
}
