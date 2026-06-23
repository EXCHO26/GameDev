using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Attack", menuName = "Abilities/Basic Attack")]
public class BasicAttackAbility : AbilityBase
{
    public override void Activate(GameObject caster, int currentLevel)
    {
        AbilityLevelData currentData = levels[currentLevel - 1];

        PlayerController player = caster.GetComponent<PlayerController>();
        
        if (player)
        {
            player.PerformComboAttack(currentData.effectAmount);
        }
    }
}